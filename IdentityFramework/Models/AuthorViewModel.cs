namespace IdentityFramework.Models
{
    public class AuthorViewModel
    {
        public Authors NewAuthor { get; set; } = new Authors();
        public IEnumerable<Authors> AuthorsList { get; set; } 
    }
}
