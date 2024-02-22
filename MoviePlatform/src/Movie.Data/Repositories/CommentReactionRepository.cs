using Movie.Core.Models;
using Movie.Core.Repositories;
using Movie.Data.DAL;

namespace Movie.Data.Repositories
{
    public class CommentReactionRepository : GenericRepository<CommentReaction>, ICommentReactionRepository
    {
        public CommentReactionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
