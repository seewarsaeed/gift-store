using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftStore.Models;

public partial class Projectcategory
{
    public decimal Id { get; set; }

    public string? Image { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Projectcategoryuser> Projectcategoryusers { get; set; } = new List<Projectcategoryuser>();
}
