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
        private string[] spoilerWords = {"death", "betrayal", "reveal", "twist", "secret", "ending", "relationship",                                      "plot", "identity","tragedy", "surprise", "resolution","resurrection",                                           "sacrifice", "redemption", "discovery", "cliffhanger", "confrontation",
                                         "revelation", "conclusion","end"};
        private string[] rudeWords = {"Fuck", "Shit", "Asshole", "Bitch", "Bastard", "Cunt", "Dick", "Douchebag", "Slut",                              "Whore","Motherfucker", "Asshat", "Twat", "Wanker", "Piss off", "Son of a bitch",
                                      "Arsehole", "Prick", "Cock", "Bullshit", "Damn", "Bollocks", "Bugger", "Fanny",             "Git", "Jerk", "Idiot", "Moron","Retard", "Piss", "Crap", "Sucker", "Dickhead",             "Pussy", "Faggot", "Freak", "Skank", "Turd", "Jackass", "Knob", "Dumbass", "Lameass",       "Bimbo", "Chode", "Shithead", "Nutjob", "Dipshit", "Screw you", "Bumblefuck",
                                      "Asswipe", "Argo"};
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

            foreach (var rude in rudeWords)
            {
                if (commentCreateDTO.Text.Trim().ToLower().Contains(rude.ToLower().Trim()))
                {
                    throw new CommentContainArgoException("Comment.Text", "Your comment contains rudeness. Please be respectful to other users.");
                }
            }

            Comment comment = new Comment
            {
                AppUserId = user.Id,
                MovieId = movie.Id,
                Text = commentCreateDTO.Text,
            };
            foreach (var spoiler in spoilerWords)
            {
                if (comment.Text.Trim().ToLower().Contains(spoiler))
                {
                    comment.IsSpoiler = true;
                    break;
                }
            }
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

        public async Task UpdateAsync(CommentUpdateDTO commentUpdateDTO)
        {
            var exist = await _commentRepository.GetAsync(x => x.Id == commentUpdateDTO.Id);
            if (exist is null) throw new CommentNotFoundException();

            exist.IsSpoiler = commentUpdateDTO.IsSpoiler;
            await _commentRepository.CommitAsync();
        }
    }
}
