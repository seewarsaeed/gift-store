using System;
using System.Collections.Generic;

namespace GiftStore.Models;

public partial class Projectstatus
{
    public decimal Id { get; set; }

    public string? Arrivedstatus { get; set; }

    public string? Paidstatus { get; set; }

    public string? Requeststatus { get; set; }

    public decimal? Presentid { get; set; }

    public string? Notifecationstatus { get; set; }

    public virtual Projectpresent? Present { get; set; }
}
