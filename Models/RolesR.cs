using System;
using System.Collections.Generic;

namespace GiftStore.Models;

public partial class RolesR
{
    public decimal Id { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<LoginR> LoginRs { get; set; } = new List<LoginR>();
}
