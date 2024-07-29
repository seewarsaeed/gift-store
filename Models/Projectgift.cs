using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftStore.Models;

public partial class Projectgift
{
    public decimal Id { get; set; }

    public string? Image { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? CategoryUsrId { get; set; }

    public decimal? Price { get; set; }

    public decimal? Userid { get; set; }

    public virtual Projectcategoryuser? CategoryUsr { get; set; }

    public virtual ICollection<Projectpresent> Projectpresents { get; set; } = new List<Projectpresent>();

    public virtual Projectuser? User { get; set; }
}
