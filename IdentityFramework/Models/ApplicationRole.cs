using Microsoft.AspNetCore.Identity;

namespace IdentityFramework.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
    }
}
