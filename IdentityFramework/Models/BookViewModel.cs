using Microsoft.AspNetCore.Mvc.Rendering;

public class BookViewModel
{
    public string Title { get; set; }
    public int? AuthorId { get; set; }
    public int? CategoryId { get; set; }
    public int Year { get; set; }
    public SelectList? Authors { get; set; }
    public SelectList? Categories { get; set; }
}
