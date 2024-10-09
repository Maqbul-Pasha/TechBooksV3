using Microsoft.EntityFrameworkCore;
using TechBooks.Models;

namespace TechBooks.Data;

public static class AuthorBooksData
{
    public static async Task<List<Book>> GetAssociatedBookList(int authorId, TechBooksContext context)
    {
        /*
        SELECT * FROM Books 
        WHERE BookId IN ( 
           SELECT DISTINCT BookId 
           FROM AuthorBooks 
           WHERE AuthorId=1
        )
        */
        var result = await (from book in context.Books
                            where (from authorBook in context.AuthorBooks
                                   where authorBook.AuthorId == authorId
                                   select authorBook.BookId).Distinct()
                                   .Contains(book.BookId)
                            select book).ToListAsync();
        return result;
    }

    public static async Task<List<Book>> GetNonAssociatedBookList(int authorId, TechBooksContext context)
    {
        /*
        SELECT * FROM Books 
        WHERE BookId NOT IN ( 
           SELECT DISTINCT BookId 
           FROM AuthorBooks 
           WHERE AuthorId=@AuthorId
        )
        */
        var result = await (from book in context.Books
                            where !(from authorBook in context.AuthorBooks
                                    where authorBook.AuthorId == authorId
                                    select authorBook.BookId).Distinct()
                                    .Contains(book.BookId)
                            select book).ToListAsync();
        return result;
    }

    public static async Task<List<Author>> GetNonAssociatedAuthorList(int bookId, TechBooksContext context)
    {
        /*
        SELECT * FROM Authors 
        WHERE AuthorId NOT IN ( 
           SELECT DISTINCT AuthorID 
           FROM AuthorBooks 
           WHERE BookId=@BookId
        )
        */
        var result = await (from author in context.Authors
                            where !(from authorBook in context.AuthorBooks
                                    where authorBook.BookId == bookId
                                    select authorBook.AuthorId).Distinct()
                                    .Contains(author.AuthorId)
                            select author).ToListAsync();
        return result;
    }

    public static async Task<List<Author>> GetAssociatedAuthorList(int bookId, TechBooksContext context)
    {
        /*
        SELECT * FROM Authors 
        WHERE AuthorId IN ( 
           SELECT DISTINCT AuthorID 
           FROM AuthorBooks 
           WHERE BookId=@BookId
        )
        */
        var result = await (from author in context.Authors
                            where (from authorBook in context.AuthorBooks
                                   where authorBook.BookId == bookId
                                   select authorBook.AuthorId).Distinct()
                                    .Contains(author.AuthorId)
                            select author).ToListAsync();
        return result;
    }

    public static async Task Insert(int authorId, int bookId, TechBooksContext context)
    {
        var authorBook = new AuthorBook { AuthorId = authorId, BookId = bookId, CreationDate = DateTime.Now };
        await context.AuthorBooks.AddAsync(authorBook);
        await context.SaveChangesAsync();
    }

    public static async Task Delete(int authorId, int bookId, TechBooksContext context)
    {
        var authorBook = new AuthorBook { AuthorId = authorId, BookId = bookId };
        context.AuthorBooks.Remove(authorBook);
        await context.SaveChangesAsync();
    }
}

