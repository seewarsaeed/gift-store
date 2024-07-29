using System;
using System.Collections.Generic;

namespace GiftStore.Models;

public partial class Projectcategoryuser
{
    public decimal Id { get; set; }

    public decimal? Userid { get; set; }

    public decimal? Categoryid { get; set; }

    public virtual Projectcategory? Category { get; set; }

    public virtual ICollection<Projectgift> Projectgifts { get; set; } = new List<Projectgift>();

    public virtual Projectuser? User { get; set; }
}
