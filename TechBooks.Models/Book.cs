using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechBooks.Models;

public class Book
{
    [Key]
    [DisplayName("Book Id")]
    public int BookId { get; set; }

    [Required]
    [DisplayName("Category")]
    public int CategoryId { get; set; }

    public Category Category { get; set; } = new Category();

    [Required]
    [StringLength(50)]
    public string Title { get; set; } = string.Empty;

    public byte[]? Picture { get; set; }
    
    public byte[]? Thumbnail { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }

    [Required]
    public DateTime CreationDate { get; set; }

    public List<Author> Authors { get; set; } = new List<Author>();
}