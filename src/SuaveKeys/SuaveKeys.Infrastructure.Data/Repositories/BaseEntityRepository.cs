using Microsoft.EntityFrameworkCore;
using SuaveKeys.Infrastructure.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuaveKeys.Infrastructure.Data.Repositories
{
    public class BaseEntityRepository<T>where T : class, new()
    {
        protected SuaveKeysContext _context;
        public BaseEntityRepository(SuaveKeysContext context)
        {
            _context = context;
        }

        protected DbSet<T> _data => _context.Set<T>();

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
