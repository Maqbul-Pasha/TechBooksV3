using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using TechBooks.Models;

namespace TechBooks.Data;

public static class BooksData
{
    private static async Task<byte[]> ResizePictureIfNeeded(byte[] imageBytes, int maxWidth)
    {
        using (var memoryStream = new MemoryStream(imageBytes))
        {
            // Load the image from the byte array
            using (var image = await Image.LoadAsync(memoryStream))
            {
                if (image.Width > maxWidth)
                {
                    // Resize the image proportionally
                    var ratio = (double)maxWidth / image.Width;
                    var newHeight = (int)(image.Height * ratio);

                    image.Mutate(x => x.Resize(maxWidth, newHeight));
                }

                // Convert the resized image back to a byte array
                using (var outputStream = new MemoryStream())
                {
                    image.Save(outputStream, new JpegEncoder()); // Saving as JPG
                    return outputStream.ToArray();
                }
            }
        }
    }

    private static async Task ProcessPictures(Book book)
    {
        if (book.Picture != null && book.Picture.Length > 0)
        {
            book.Picture = await ResizePictureIfNeeded(book.Picture, 500);
            book.Thumbnail = await ResizePictureIfNeeded(book.Picture, 100);
        }
    }

    public static async Task Insert(Book book, TechBooksContext context)
    {
        book.CreationDate = DateTime.Now;
        context.Attach(book.Category); // NECESSARY TO AVOID AN EXCEPTION
        context.Books.Add(book);
        await context.SaveChangesAsync();
    }

    public static async Task Update(Book book, TechBooksContext context)
    {
        context.Attach(book.Category); // NECESSARY TO AVOID CHANGING THE CATEGORY NAME TO "Temp Name"
        var entity = context.Books.Update(book);
        entity.Property(c => c.CreationDate).IsModified = false;
        entity.Property(c => c.Thumbnail).IsModified = book.Thumbnail != null;
        entity.Property(c => c.Picture).IsModified = book.Picture != null;

        /*
         *
         */
        await context.SaveChangesAsync();
    }

    public static async Task<Book?> GetBook(int bookId, TechBooksContext context)
    {
        return await context.Books.FindAsync(bookId);
    }

    public static async Task<List<Book>> GetList(TechBooksContext context)
    {
        return await context.Books.ToListAsync();
    }

    public static async Task Delete(Book book, TechBooksContext context)
    {
        //book.Category = null!; // NECESSARY TO AVOID CREATING A BLANK CATEGORY
        context.Books.Remove(book);
        await context.SaveChangesAsync();
    }

    public static async Task<bool> HasAuthors(Book book, TechBooksContext context)
    {
        return await context.AuthorBooks.AnyAsync(ab => ab.BookId == book.BookId);
    }
}


