using Microsoft.AspNetCore.Http;
using Movie.Business.CustomExceptions.CommentExceptions;
using Movie.Business.CustomExceptions.UserException;
using Movie.Business.Services.Interfaces;
using Movie.Core.Models;
using Movie.Core.Repositories;
using System.Linq.Expressions;

namespace Movie.Business.Services.Implementations
{
    public class CommentReactionService : ICommentReactionService
    {
        private readonly ICommentReactionRepository _reactionRepository;
        private readonly ICommentService _commentService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IAccountService _accountService;

        public CommentReactionService(ICommentReactionRepository reactionRepository,
                                      ICommentService commentService,
                                      IHttpContextAccessor httpContext,
                                      IAccountService accountService)
        {
            _reactionRepository = reactionRepository;
            _commentService = commentService;
            _httpContext = httpContext;
            _accountService = accountService;
        }

        public async Task<bool?> Dislike(int id)
        {
            var comment = await _commentService.GetCommentAsync(x => x.Id == id);
            if (comment == null) throw new CommentNotFoundException();
            var user = await _accountService.GetUserByNameAsync(_httpContext.HttpContext.User.Identity.Name);
            if (user is null) throw new UserNotFoundException();

            bool? check = null;
            var commentReaction = await _reactionRepository.GetAsync(x => x.AppUserId == user.Id
                                                                       && x.CommentId == comment.Id);
            if (commentReaction is not null && !commentReaction.IsLike)
            {
                comment.Dislike--;
                _reactionRepository.Delete(commentReaction);
                check = false;
            }
            else if (commentReaction is not null && commentReaction.IsLike)
            {
                comment.Like--;
                comment.Dislike++;
                commentReaction.IsLike = false;
            }
            else
            {
                check = true;
                var newReaction = new CommentReaction()
                {
                    AppUserId = user.Id,
                    CommentId = comment.Id,
                    IsLike = false,
                };
                comment.Dislike++;
                await _reactionRepository.CreateAsync(newReaction);
            }
            await _reactionRepository.CommitAsync();
            return check;
        }

        public async Task<bool?> Like(int id)
        {
            var comment = await _commentService.GetCommentAsync(x => x.Id == id);
            if (comment == null) throw new CommentNotFoundException();
            var user = await _accountService.GetUserByNameAsync(_httpContext.HttpContext.User.Identity.Name);
            if (user is null) throw new UserNotFoundException();
            bool? check = null;
            var commentReaction = await _reactionRepository.GetAsync(x => x.AppUserId == user.Id
                                                                       && x.CommentId == comment.Id);
            if (commentReaction is not null && commentReaction.IsLike)
            {
                comment.Like--;
                _reactionRepository.Delete(commentReaction);
                check = false;
            }
            else if (commentReaction is not null && !commentReaction.IsLike)
            {
                commentReaction.IsLike = true;
                comment.Dislike--;
                comment.Like++;
            }
            else
            {
                check = true;
                var newReaction = new CommentReaction()
                {
                    AppUserId = user.Id,
                    CommentId = comment.Id,
                    IsLike = true,
                };
                comment.Like++;
                await _reactionRepository.CreateAsync(newReaction);
            }
            await _reactionRepository.CommitAsync();
            return check;
        }
    }
}
