using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SuaveKeys.Api.Hubs;
using SuaveKeys.Api.Providers;
using SuaveKeys.Core.Business.Services;
using SuaveKeys.Core.Data.Providers;
using SuaveKeys.Core.Data.Repositories;
using SuaveKeys.Core.Models.Configuration;
using SuaveKeys.Core.Models.Entities;
using SuaveKeys.Infrastructure.Business.Services;
using SuaveKeys.Infrastructure.Data.Contexts;
using SuaveKeys.Infrastructure.Data.Providers;
using SuaveKeys.Infrastructure.Data.Repositories;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SuaveKeys.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson();
            services.AddControllers();
            services.AddSignalR();
            AddBearerAuth(services);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Suave Keys API", Version = "v1" });
                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer"
                });
                c.OperationFilter<AuthenticationRequirementsOperationFilter>();
            });

            services.AddDbContext<SuaveKeysContext>(options =>
                  options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
               );

            services.Configure<AuthSettings>(Configuration.GetSection(nameof(AuthSettings)));
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();


            // our code!
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IAuthClientRepository, AuthClientRepository>();
            services.AddScoped<IAuthorizationCodeRepository, AuthorizationCodeRepository>();
            services.AddScoped<IUserKeyboardProfileRepository, UserKeyboardProfileRepository>();

            services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            services.AddScoped<IHashProvider, SecureHashProvider>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IKeyboardProfileService, KeyboardProfileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SuaveKeys API V1");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<KeyboardHub>("/keyboard");
            });
        }

        private void AddBearerAuth(IServiceCollection services)
        {
            var authSettings = Configuration.GetSection("AuthSettings");
            var symmetricKeyAsBase64 = authSettings["EncodingSecret"];
            var keyByteArray = Encoding.UTF8.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!  
                ValidateIssuerSigningKey = true,
                ValidIssuer = authSettings["Issuer"],
                ValidAudience = authSettings["Issuer"],
                IssuerSigningKey = signingKey,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = tokenValidationParameters;

                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/keyboard")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }

    public class AuthenticationRequirementsOperationFilter : IOperationFilter
    {

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Security == null)
                operation.Security = new List<OpenApiSecurityRequirement>();


            var scheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" } };
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [scheme] = new List<string>()
            });
        }
    }
}
