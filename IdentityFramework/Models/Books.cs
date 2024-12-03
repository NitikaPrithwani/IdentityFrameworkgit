using System.ComponentModel.DataAnnotations;

namespace IdentityFramework.Models
{
    public class Books
    {
        [Key]
        public int BookId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int? AuthorId { get; set; }
        public Authors? Author { get; set; }
        [Required]
        public int? CategoryId { get; set; }
        public BookCategory? Category { get; set; }
        [Required]
        public int Year { get; set; }
        public ICollection<BorrowingRecords> BorrowingRecords { get; set; } = new List<BorrowingRecords>();
    }

}
