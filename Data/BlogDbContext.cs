using Microsoft.EntityFrameworkCore;
using Blog.Models;

namespace Blog.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Veritabanı model yapılandırmalarını tanımlar
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Blog yazısı tablosu yapılandırması
            modelBuilder.Entity<BlogPost>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.Summary).HasMaxLength(500);
                // Yorumlar ile ilişki: Bir blog yazısı silindiğinde yorumları da silinir
                entity.HasMany(e => e.Comments)
                      .WithOne(e => e.BlogPost)
                      .HasForeignKey(e => e.BlogPostId)
                      .OnDelete(DeleteBehavior.Cascade);
                // Kullanıcı ile ilişki: Bir kullanıcı silindiğinde blog yazıları da silinir
                entity.HasOne(e => e.User)
                      .WithMany(e => e.BlogPosts)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Yorum tablosu yapılandırması
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AuthorName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(1000);
            });

            // Kullanıcı tablosu yapılandırması
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
                // Kullanıcı adı ve e-posta benzersiz olmalı
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
    }
}

