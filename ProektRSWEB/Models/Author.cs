using System.ComponentModel.DataAnnotations;

namespace ProektRSWEB.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Required]
        public string FirstName { get; set; }
        [StringLength(50)]
        [Required]
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? BirthDate { get; set; }
        [StringLength(50)]
        public string? Nationality { get; set; }
        [StringLength(50)]
        public string? Gender { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
