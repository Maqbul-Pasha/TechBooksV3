using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TechBooks.Models;

public class Author
{
    [Key]
    [DisplayName("Author Id")]
    public int AuthorId { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public DateTime CreationDate { get; set; }

    public List<Book> Books { get; set; } = new List<Book>();
}
