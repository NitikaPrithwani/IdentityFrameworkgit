using IdentityFramework.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityFramework.Models
{
    public class BorrowingRecords
    {
        [Key]
        public int RecordId { get; set; }

        [ForeignKey("User")]
        public string Id { get; set; }  
        public ApplicationUser User { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Books Book { get; set; } 

        public DateTime IssueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
