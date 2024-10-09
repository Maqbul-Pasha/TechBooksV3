using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TechBooks.Models;

public class Category
{
    [Key]
    [DisplayName("Category Id")]
    public int CategoryId { get; set; }

    [Required]
    [StringLength(50)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime CreationDate { get; set; }

    public List<Book> Books { get; set; } = new List<Book>();
}
