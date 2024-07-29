using System;
using System.Collections.Generic;

namespace GiftStore.Models;

public partial class CustomerR
{
    public decimal Id { get; set; }

    public string? Fname { get; set; }

    public string? Lname { get; set; }

    public string? ImagePath { get; set; }

    public virtual ICollection<LoginR> LoginRs { get; set; } = new List<LoginR>();

    public virtual ICollection<ProductCustomerR> ProductCustomerRs { get; set; } = new List<ProductCustomerR>();
}
