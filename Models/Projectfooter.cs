using System;
using System.Collections.Generic;

namespace GiftStore.Models;

public partial class Projectfooter
{
    public decimal Id { get; set; }

    public string? Copyright { get; set; }

    public virtual ICollection<Projecthome> Projecthomes { get; set; } = new List<Projecthome>();
}
