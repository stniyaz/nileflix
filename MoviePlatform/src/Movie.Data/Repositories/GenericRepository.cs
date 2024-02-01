using Microsoft.EntityFrameworkCore;
using Movie.Core.Models;
using Movie.Core.Repositories;
using Movie.Data.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        public DbSet<T> Table => _context.Set<T>();

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(T entity)
        {
            await Table.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            Table.Remove(entity);
        }

        public IQueryable<T> GetAllAsync(Expression<Func<T, bool>>? expression = null, params string[]? includes)
        {
            return _getQuery(expression, includes);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? expression = null, params string[]? includes)
        {
            return await _getQuery(expression, includes).FirstOrDefaultAsync();
        }

        public IQueryable<T> _getQuery(Expression<Func<T, bool>>? expression, params string[] includes)
        {
            var query = Table.AsQueryable();
            query = expression is not null ? query.Where(expression) : query;

            if (includes is not null)
            {
                query = includes.Aggregate(query, (currentQuery, include) => currentQuery.Include(include));
            }
            return query;
        }
    }
}
