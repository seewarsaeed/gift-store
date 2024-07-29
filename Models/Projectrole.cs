using System;
using System.Collections.Generic;

namespace GiftStore.Models;

public partial class Projectrole
{
    public decimal Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Projectuser> Projectusers { get; set; } = new List<Projectuser>();
}
