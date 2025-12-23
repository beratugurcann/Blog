using Blog.Models;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Data
{
    public static class DbInitializer
    {
        /// <summary>
        /// Veritabanını başlatır ve gerekli kontrolleri yapar
        /// </summary>
        public static void Initialize(BlogDbContext context)
        {
            try
            {
                // Kullanıcı varsa başlatma işlemini atla
                if (context.Users.Any())
                {
                    return;
                }
            }
            catch
            {
                // Veritabanı bağlantısı yoksa sessizce devam et
            }
        }

        /// <summary>
        /// Şifreyi SHA256 algoritması ile hashler
        /// </summary>
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}

