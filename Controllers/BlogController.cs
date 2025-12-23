using Microsoft.AspNetCore.Mvc;
using Blog.Data;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogDbContext _context;

        public BlogController(BlogDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Blog yazısı detay sayfasını gösterir
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var blogPost = await _context.BlogPosts
                .Include(b => b.Comments.OrderByDescending(c => c.CreatedDate))
                .FirstOrDefaultAsync(b => b.Id == id);

            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        /// <summary>
        /// Blog yazısına yeni yorum ekler
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            if (comment.BlogPostId <= 0)
            {
                return BadRequest();
            }

            var blogPostExists = await _context.BlogPosts.AnyAsync(b => b.Id == comment.BlogPostId);
            if (!blogPostExists)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                comment.CreatedDate = DateTime.Now;
                comment.AuthorName = comment.AuthorName?.Trim() ?? string.Empty;
                comment.Email = comment.Email?.Trim().ToLower() ?? string.Empty;
                comment.Content = comment.Content?.Trim() ?? string.Empty;
                
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = comment.BlogPostId });
            }

            var blogPost = await _context.BlogPosts
                .Include(b => b.Comments.OrderByDescending(c => c.CreatedDate))
                .FirstOrDefaultAsync(b => b.Id == comment.BlogPostId);

            if (blogPost == null)
            {
                return NotFound();
            }

            return View("Details", blogPost);
        }
    }
}

