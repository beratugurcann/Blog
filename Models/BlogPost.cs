using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur")]
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "İçerik zorunludur")]
        public string Content { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Özet en fazla 500 karakter olabilir")]
        public string? Summary { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public List<Comment> Comments { get; set; } = new();
    }
}

