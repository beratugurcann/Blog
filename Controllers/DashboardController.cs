using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Blog.Data;
using Blog.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blog.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly BlogDbContext _context;

        public DashboardController(BlogDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Giriş yapmış kullanıcının ID'sini döndürür
        /// </summary>
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        /// <summary>
        /// Kullanıcının kendi blog yazılarını listeler
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var blogPosts = await _context.BlogPosts
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedDate)
                .ToListAsync();

            return View(blogPosts);
        }

        /// <summary>
        /// Yeni blog yazısı oluşturma sayfasını gösterir
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Yeni blog yazısı oluşturur
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                blogPost.Title = blogPost.Title?.Trim() ?? string.Empty;
                blogPost.Content = blogPost.Content?.Trim() ?? string.Empty;
                blogPost.Summary = blogPost.Summary?.Trim();
                blogPost.CreatedDate = DateTime.Now;
                blogPost.UserId = GetCurrentUserId();
                _context.BlogPosts.Add(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(blogPost);
        }

        /// <summary>
        /// Blog yazısı düzenleme sayfasını gösterir (sadece kendi yazıları)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = GetCurrentUserId();
            var blogPost = await _context.BlogPosts
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        /// <summary>
        /// Blog yazısını günceller (sadece kendi yazıları)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogPost blogPost)
        {
            var userId = GetCurrentUserId();
            
            if (id != blogPost.Id)
            {
                return NotFound();
            }

            var existingPost = await _context.BlogPosts
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

            if (existingPost == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingPost.Title = blogPost.Title?.Trim() ?? string.Empty;
                    existingPost.Content = blogPost.Content?.Trim() ?? string.Empty;
                    existingPost.Summary = blogPost.Summary?.Trim();
                    existingPost.UpdatedDate = DateTime.Now;
                    _context.Update(existingPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(blogPost.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction("Index");
            }

            return View(blogPost);
        }

        /// <summary>
        /// Blog yazısı silme onay sayfasını gösterir (sadece kendi yazıları)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId();
            var blogPost = await _context.BlogPosts
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        /// <summary>
        /// Blog yazısını siler (sadece kendi yazıları)
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetCurrentUserId();
            var blogPost = await _context.BlogPosts
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

            if (blogPost != null)
            {
                _context.BlogPosts.Remove(blogPost);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Kullanıcının blog yazılarına yapılan yorumları listeler
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Comments()
        {
            var userId = GetCurrentUserId();
            var comments = await _context.Comments
                .Include(c => c.BlogPost)
                .Where(c => c.BlogPost != null && c.BlogPost.UserId == userId)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();

            return View(comments);
        }

        /// <summary>
        /// Yorumu siler (sadece kendi yazılarının yorumları)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var userId = GetCurrentUserId();
            var comment = await _context.Comments
                .Include(c => c.BlogPost)
                .FirstOrDefaultAsync(c => c.Id == id && c.BlogPost != null && c.BlogPost.UserId == userId);

            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Comments");
        }

        /// <summary>
        /// Blog yazısının var olup olmadığını kontrol eder
        /// </summary>
        private bool BlogPostExists(int id)
        {
            return _context.BlogPosts.Any(e => e.Id == id);
        }
    }
}

