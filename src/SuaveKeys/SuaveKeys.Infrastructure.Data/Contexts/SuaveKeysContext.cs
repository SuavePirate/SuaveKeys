using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuaveKeys.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Infrastructure.Data.Contexts
{
    public class SuaveKeysContext : IdentityDbContext<SuaveKeysUser, SuaveKeysUserRole, string>
    {
        public SuaveKeysContext(DbContextOptions options) : base(options)
        {
        }
    }
}
