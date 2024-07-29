using System;
using System.Collections.Generic;

namespace GiftStore.Models;

public partial class ProductR
{
    public decimal Id { get; set; }

    public string? Namee { get; set; }

    public decimal? Sale { get; set; }

    public decimal? Price { get; set; }

    public decimal? CategoryId { get; set; }

    public virtual CategortR? Category { get; set; }

    public virtual ICollection<ProductCustomerR> ProductCustomerRs { get; set; } = new List<ProductCustomerR>();
}
