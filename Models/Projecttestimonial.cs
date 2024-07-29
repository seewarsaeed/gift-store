using System;
using System.Collections.Generic;

namespace GiftStore.Models;

public partial class Projecttestimonial
{
    public decimal Id { get; set; }

    public string? Satatus { get; set; }

    public string? Description { get; set; }

    public decimal? UserId { get; set; }

    public virtual ICollection<Projecthome> Projecthomes { get; set; } = new List<Projecthome>();

    public virtual Projectuser? User { get; set; }
}
