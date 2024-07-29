using System;
using System.Collections.Generic;

namespace GiftStore.Models;

public partial class CategortR
{
    public decimal Id { get; set; }

    public string? CategoryName { get; set; }

    public string? ImagePath { get; set; }

    public virtual ICollection<ProductR> ProductRs { get; set; } = new List<ProductR>();
}
