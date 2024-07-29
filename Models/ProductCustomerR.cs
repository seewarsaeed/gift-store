using System;
using System.Collections.Generic;

namespace GiftStore.Models;

public partial class ProductCustomerR
{
    public decimal Id { get; set; }

    public decimal? ProductId { get; set; }

    public decimal? CudtomerId { get; set; }

    public decimal? Quantity { get; set; }

    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    public virtual CustomerR? Cudtomer { get; set; }

    public virtual ProductR? Product { get; set; }
}
