using System.ComponentModel.DataAnnotations;

namespace ProektRSWEB.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        public string Title { get; set; }
        public int? YearPublished {  get; set; }
        public int? NumberOfPages { get; set; }
        public string? Description { get; set; }
        public string? Publisher { get; set; }
        public string? FrontPage { get; set; }
        public string? DownloadURL { get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<UserBooks>? UserBooks { get; set; }
        public ICollection<BookGenre>? BookGenre { get; set; }

    }
}
