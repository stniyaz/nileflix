using Movie.Business.DTOs.ComentDTOs;
using Movie.Core.Models;
using System.Linq.Expressions;

namespace Movie.Business.Services.Interfaces
{
    public interface ICommentService
    {
        Task CreateAsync(CommentCreateDTO commentCreateDTO);
        Task<List<Comment>> GetCommentAsync(Expression<Func<Comment, bool>>? expression = null, params string[]? includes);
    }
}
