using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Movie.Business.CustomExceptions.CommentExceptions;
using Movie.Business.CustomExceptions.CommonExceptions;
using Movie.Business.CustomExceptions.MoiveExceptions;
using Movie.Business.CustomExceptions.UserException;
using Movie.Business.DTOs.ComentDTOs;
using Movie.Business.Services.Interfaces;
using Movie.Core.Models;
using Movie.Core.Repositories;
using System.Linq.Expressions;

namespace Movie.Business.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMovieService _movieService;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContext;

        public CommentService(ICommentRepository commentRepository,
                              IMovieService movieService,
                              IAccountService accountService,
                              IHttpContextAccessor httpContext)
        {
            _commentRepository = commentRepository;
            _movieService = movieService;
            _accountService = accountService;
            _httpContext = httpContext;
        }
        public async Task<Comment> CreateAsync(CommentCreateDTO commentCreateDTO)
        {
            var user = await _accountService.GetUserByNameAsync(_httpContext.HttpContext.User.Identity.Name);
            if (user is null) throw new UserNotFoundException();

            var movie = await _movieService.GetAsync(x => x.Id == commentCreateDTO.MovieId);
            if (movie is null) throw new MovieNotFoundException();

            Comment comment = new Comment
            {
                AppUserId = user.Id,
                MovieId = movie.Id,
                Text = commentCreateDTO.Text,
            };
            await _commentRepository.CreateAsync(comment);
            await _commentRepository.CommitAsync();

            return await _commentRepository.Table.Where(x => x.Text == commentCreateDTO.Text).Include(x => x.AppUser).FirstOrDefaultAsync();
        }

        public async Task<List<Comment>> GetCommentsAsync(Expression<Func<Comment, bool>>? expression = null, params string[]? includes)
        {
            return await _commentRepository.GetAllAsync(expression, includes).ToListAsync();
        }

        public async Task<List<Comment>> GetCommentsByAsync(string search)
        {
            var comments = await _commentRepository.GetAllAsync(null, "Movie", "AppUser").ToListAsync();
            return await SearchComment(comments, search);
        }

        public async Task DeleteAsync(int id)
        {
            var wantedComment = await _commentRepository.GetAsync(x => x.Id == id);
            if (wantedComment is null) throw new CommentNotFoundException();

            _commentRepository.Delete(wantedComment);
            await _commentRepository.CommitAsync();
        }

        private async Task<List<Comment>> SearchComment(List<Comment> comments, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                if (search.Length >= 2)
                    comments = comments.Where(x => x.Text.Trim().ToLower().Contains(search.Trim())).ToList();
                else
                    throw new InvalidSearchException();
            }
            return comments;
        }

        public async Task<Comment> GetCommentAsync(Expression<Func<Comment, bool>>? expression = null, params string[]? includes)
        {
            return await _commentRepository.GetAsync(expression, includes);
        }
    }
}
