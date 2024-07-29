using System;
using System.Collections.Generic;

namespace GiftStore.Models;

public partial class LoginR
{
    public decimal Id { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public decimal? RoleId { get; set; }

    public decimal? CustomerId { get; set; }

    public virtual CustomerR? Customer { get; set; }

    public virtual RolesR? Role { get; set; }
}
