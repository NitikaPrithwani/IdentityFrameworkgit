using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace IdentityFramework.Models
{
    public class IssueBookViewModel
    {
        [Required(ErrorMessage = "Please select a user.")]
        public string SelectedUserId { get; set; }

        [Required(ErrorMessage = "Please select a book.")]
        public int SelectedBookId { get; set; }

        public List<SelectListItem> Users { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Books { get; set; } = new List<SelectListItem>();
    }
}
