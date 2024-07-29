using System;
using System.Collections.Generic;

namespace GiftStore.Models;

public partial class Projectpayment
{
    public decimal Id { get; set; }

    public string? Cardnumber { get; set; }

    public DateTime? Expirationdate { get; set; }

    public string? Cvvcvc { get; set; }

    public decimal? Availablebalance { get; set; }

    public decimal? Userid { get; set; }

    public virtual Projectuser? User { get; set; }
}
