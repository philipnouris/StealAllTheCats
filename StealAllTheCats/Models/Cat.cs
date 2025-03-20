using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StealAllTheCats.Models;

public partial class Cat
{
    public int Id { get; set; }

    [Required]
    public string CatId { get; set; } = null!;
    [Range(10,5000)]
    public int Width { get; set; }
    [Range(10, 5000)]
    public int Height { get; set; }
    [Required]
    public string ImageUrl { get; set; } = null!;
    
    public DateTime? Created { get; set; }

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
