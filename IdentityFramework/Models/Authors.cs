using System.ComponentModel.DataAnnotations;

namespace IdentityFramework.Models
{
    public class Authors
    {
        [Key]
        public int AuthorId {  get; set; }
        [Required(ErrorMessage = "Author name is required")]
        public string AuthorName { get; set; }

        [Required(ErrorMessage = "Biography is required")]
        public string Biography {  get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public ICollection<Books> Books { get; set; }
    }
}
