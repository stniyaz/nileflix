using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        public async Task CreateAsync(CommentCreateDTO commentCreateDTO)
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
        }

        public async Task<List<Comment>> GetCommentAsync(Expression<Func<Comment, bool>>? expression = null, params string[]? includes)
        {
            return await _commentRepository.GetAllAsync(expression, includes).ToListAsync();
        }
    }
}
