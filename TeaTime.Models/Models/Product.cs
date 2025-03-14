using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaTime.Models;

public class Product
{
    private string name = string.Empty;

    [Key]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Size { get; set; }
    [Required]
    [Range(1, 10000)]
    public double Price { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    [NotMapped]
    [ScaffoldColumn(false)]
    public Category? Category { get; set; } = null;
    [NotMapped]
    [ScaffoldColumn(false)]
    public string? CategoryName { get; set; } = string.Empty;
}


