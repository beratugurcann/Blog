using Microsoft.AspNetCore.Mvc;
using Blog.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlogDbContext _context;

        public HomeController(BlogDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Ana sayfayı gösterir ve tüm blog yazılarını listeler
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var blogPosts = await _context.BlogPosts
                .OrderByDescending(b => b.CreatedDate)
                .ToListAsync();

            return View(blogPosts);
        }

        /// <summary>
        /// Hakkında sayfasını gösterir
        /// </summary>
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Hata sayfasını gösterir
        /// </summary>
        public IActionResult Error()
        {
            return View();
        }
    }
}

