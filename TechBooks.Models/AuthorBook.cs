using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechBooks.Models;

[PrimaryKey(nameof(AuthorId), nameof(BookId))]
public class AuthorBook
{
    public int AuthorId { get; set; }

    public int BookId { get; set; }

    [Required]
    public DateTime CreationDate { get; set; }
}
