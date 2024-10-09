using TechBooks.Models;

namespace TechBooks.WebUI.Models;

public class BookAuthorsViewModel
{
    public Book Book { get; set; } = new Book();
    public List<Author> AssociatedAuthors { get; set; } = new List<Author>();
    public List<Author> NonAssociatedAuthors { get; set; } = new List<Author>();
}

