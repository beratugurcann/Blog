using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "İsim zorunludur")]
        [StringLength(100, ErrorMessage = "İsim en fazla 100 karakter olabilir")]
        public string AuthorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yorum zorunludur")]
        [StringLength(1000, ErrorMessage = "Yorum en fazla 1000 karakter olabilir")]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int BlogPostId { get; set; }
        public BlogPost? BlogPost { get; set; }
    }
}

