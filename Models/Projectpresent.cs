using System;
using System.Collections.Generic;

namespace GiftStore.Models;

public partial class Projectpresent
{
    public decimal Id { get; set; }

    public decimal? Userid { get; set; }

    public string? Reciveraddress { get; set; }

    public decimal? Giftsid { get; set; }

    public DateTime? Requestdate { get; set; }

    public virtual Projectgift? Gifts { get; set; }

    public virtual ICollection<Projectstatus> Projectstatuses { get; set; } = new List<Projectstatus>();

    public virtual Projectuser? User { get; set; }
}
