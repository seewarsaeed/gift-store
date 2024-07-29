using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftStore.Models;

public partial class Projectheader
{
    public decimal Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Projecthome> Projecthomes { get; set; } = new List<Projecthome>();
}
