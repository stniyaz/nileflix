using Movie.Business.DTOs.ComentDTOs;
using Movie.Core.Models;
using System.Linq.Expressions;

namespace Movie.Business.Services.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> CreateAsync(CommentCreateDTO commentCreateDTO);
        Task<List<Comment>> GetCommentsAsync(Expression<Func<Comment, bool>>? expression = null, params string[]? includes);
        Task<Comment> GetCommentAsync(Expression<Func<Comment, bool>>? expression = null, params string[]? includes);
        Task<List<Comment>> GetCommentsByAsync(string search);
        Task DeleteAsync(int id);
    }
}
