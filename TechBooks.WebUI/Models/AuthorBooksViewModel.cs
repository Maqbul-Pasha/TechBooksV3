using TechBooks.Models;

namespace TechBooks.WebUI.Models;

public class AuthorBooksViewModel
{
    public Author Author { get; set; } = new Author();
    public List<Book> AssociatedBooks { get; set; } = new List<Book>();
    public List<Book> NonAssociatedBooks { get; set; } = new List<Book>();
}

