using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StealAllTheCats.Models;

public partial class Tag
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public DateTime? Created { get; set; }

    public virtual ICollection<Cat> Cats { get; set; } = new List<Cat>();
}
