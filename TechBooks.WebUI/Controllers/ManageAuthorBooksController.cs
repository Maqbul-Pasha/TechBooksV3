using Microsoft.AspNetCore.Mvc;
using TechBooks.Data;
using TechBooks.Models;
using TechBooks.WebUI.Models;
using Microsoft.AspNetCore.Authorization;

namespace TechBooks.WebUI.Controllers;

[Authorize]
public class ManageAuthorBooksController : Controller
{
    private readonly TechBooksContext _context;

    public ManageAuthorBooksController(TechBooksContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? id)
    {
        if (id == null) return RedirectToAction("Index", "NotFound", new { entity = "Author", backUrl = "/ManageAuthors/" });

        Author? author = null;
        List<Book> listOfAssociatedBooks = new List<Book>();
        List<Book> listOfNonAssociatedBooks = new List<Book>();

        try
        {
            author = await AuthorsData.GetAuthor((int)id, _context);
            if (author == null) return RedirectToAction("Index", "NotFound", new { entity = "Author", backUrl = "/ManageAuthors/" });
            listOfAssociatedBooks = await AuthorBooksData.GetAssociatedBookList((int)id, _context);
            listOfNonAssociatedBooks = await AuthorBooksData.GetNonAssociatedBookList((int)id, _context);
        }
        catch (Exception ex)
        {
            TempData["DangerMessage"] = ex.Message;
        }

        var myViewModel = new AuthorBooksViewModel();
        myViewModel.Author = author!;
        myViewModel.AssociatedBooks = listOfAssociatedBooks;
        myViewModel.NonAssociatedBooks = listOfNonAssociatedBooks;

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
        return RedirectToAction(nameof(Index), new { id = authorId });
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
        return RedirectToAction(nameof(Index), new { id = authorId });
    }
}

