using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Business.CustomExceptions.CommentExceptions;
using Movie.Business.CustomExceptions.CommonExceptions;
using Movie.Business.DTOs.ComentDTOs;
using Movie.Business.Services.Interfaces;

namespace Movie.MVC.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentController(ICommentService commentService,
                                 IMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string search)
        {
            try
            {
                ViewBag.Search = search;
                var comments = await _commentService.GetCommentsByAsync(search);
                return View(comments);
            }
            catch (InvalidSearchException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _commentService.DeleteAsync(id);
                return Ok();
            }
            catch (CommentNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IActionResult> Show(int id)
        {
            var wanted = await _commentService.GetCommentAsync(x => x.Id == id);
            if (wanted is null) return NotFound();
            var dto = _mapper.Map<CommentUpdateDTO>(wanted);
            return View(dto);
        }
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> Show(CommentUpdateDTO dto)
        {
            try
            {
                await _commentService.UpdateAsync(dto);
            }
            catch (CommentNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("index");
        }
    }
}
