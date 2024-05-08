using System.ComponentModel.DataAnnotations;

namespace ProektRSWEB.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string GenreName { get; set; }
        public ICollection<BookGenre>? BookGenres { get; set; }
    }
}
