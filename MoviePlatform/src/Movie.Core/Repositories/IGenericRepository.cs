using Microsoft.EntityFrameworkCore;
using Movie.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity, new()
    {
        DbSet<T> Table { get; }

        Task CreateAsync(T entity);
        void Delete(T entity);
        Task<T> GetAsync(Expression<Func<T, bool>>? expression = null,params string[]? includes);
        IQueryable<T> GetAllAsync(Expression<Func<T, bool>>? expression = null, params string[]? includes);
        Task<int> CommitAsync();
    }
}
