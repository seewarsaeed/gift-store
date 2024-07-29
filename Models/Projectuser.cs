using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftStore.Models;

public partial class Projectuser
{
    public decimal Id { get; set; }

    public string? Image { get; set; }
    [NotMapped]
    public virtual IFormFile? ImageFile { get; set; }
    public string? Fname { get; set; }

    public string? Lname { get; set; }

    public string? Email { get; set; }

    public string? Pnumber { get; set; }

    public string? Address { get; set; }

    public string? Status { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public decimal? Roleid { get; set; }

    public virtual ICollection<Projectcategoryuser> Projectcategoryusers { get; set; } = new List<Projectcategoryuser>();

    public virtual ICollection<Projectgift> Projectgifts { get; set; } = new List<Projectgift>();

    public virtual ICollection<Projectpayment> Projectpayments { get; set; } = new List<Projectpayment>();

    public virtual ICollection<Projectpresent> Projectpresents { get; set; } = new List<Projectpresent>();

    public virtual ICollection<Projecttestimonial> Projecttestimonials { get; set; } = new List<Projecttestimonial>();

    public virtual Projectrole? Role { get; set; }
}
