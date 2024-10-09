using Microsoft.AspNetCore.Mvc;
using TechBooks.Data;
using TechBooks.Models;
using TechBooks.WebUI.Models;
using Microsoft.AspNetCore.Authorization;


namespace TechAuthors.WebUI.Controllers;

[Authorize]
public class ManageBookAuthorsController : Controller
{
    private readonly TechBooksContext _context;

    public ManageBookAuthorsController(TechBooksContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? id)
    {
        if (id == null) return RedirectToAction("Index", "NotFound", new { entity = "Book", backUrl = "/ManageBooks/" });

        Book? book = null;
        List<Author> listOfAssociatedAuthors = new List<Author>();
        List<Author> listOfNonAssociatedAuthors = new List<Author>();

        try
        {
            book = await BooksData.GetBook((int)id, _context);
            if (book == null) return RedirectToAction("Index", "NotFound", new { entity = "Book", backUrl = "/ManageBooks/" });
            listOfAssociatedAuthors = await AuthorBooksData.GetAssociatedAuthorList((int)id, _context);
            listOfNonAssociatedAuthors = await AuthorBooksData.GetNonAssociatedAuthorList((int)id, _context);
        }
        catch (Exception ex)
        {
            TempData["DangerMessage"] = ex.Message;
        }

        var myViewModel = new BookAuthorsViewModel();
        myViewModel.Book = book!;
        myViewModel.AssociatedAuthors = listOfAssociatedAuthors;
        myViewModel.NonAssociatedAuthors = listOfNonAssociatedAuthors;

        return View(myViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int authorId, int bookId)
    {
        try
        {
            await AuthorBooksData.Insert(authorId, bookId, _context);
        }
        catch (Exception ex)
        {
            TempData["DangerMessage"] = ex.Message;
        }
        return RedirectToAction(nameof(Index), new { id = bookId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int authorId, int bookId)
    {
        try
        {
            await AuthorBooksData.Delete(authorId, bookId, _context);
        }
        catch (Exception ex)
        {
            TempData["DangerMessage"] = ex.Message;
        }
        return RedirectToAction(nameof(Index), new { id = bookId });
    }
}
