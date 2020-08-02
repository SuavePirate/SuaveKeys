using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuaveKeys.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Infrastructure.Data.Contexts
{
    public class SuaveKeysContext : DbContext
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<AuthClient> AuthClients { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AuthorizationCode> AuthorizationCodes { get; set; }
        public DbSet<UserKeyboardProfile> KeyboardProfiles { get; set; }

        public SuaveKeysContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RefreshToken>().HasAlternateKey(r => r.Token);
        }
    }
}
