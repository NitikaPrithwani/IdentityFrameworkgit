using System.ComponentModel.DataAnnotations;

namespace IdentityFramework.Models
{
    public class BookCategory
    {
        [Key]
        public int CategoryId { get; set; }          
        public string CategoryName { get; set; }     
        public ICollection<Books> Books { get; set; }
    }
}
