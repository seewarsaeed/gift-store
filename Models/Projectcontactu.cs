using System;
using System.Collections.Generic;

namespace GiftStore.Models;

public partial class Projectcontactu
{
    public decimal Id { get; set; }

    public string? Email { get; set; }

    public string? Pnumber { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Projecthome> Projecthomes { get; set; } = new List<Projecthome>();
}
