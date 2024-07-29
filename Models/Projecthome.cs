using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftStore.Models;

public partial class Projecthome
{
    public decimal Id { get; set; }

    public string? Logo { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }

    public string? Name { get; set; }

    public decimal? Backgroundid { get; set; }

    public decimal? Contactusid { get; set; }

    public decimal? Headerid { get; set; }

    public decimal? Aboutusid { get; set; }

    public decimal? Footerid { get; set; }

    public decimal? Testimonialid { get; set; }

    public virtual Projectaboutu? Aboutus { get; set; }

    public virtual Projectbackground? Background { get; set; }

    public virtual Projectcontactu? Contactus { get; set; }

    public virtual Projectfooter? Footer { get; set; }

    public virtual Projectheader? Header { get; set; }

    public virtual Projecttestimonial? Testimonial { get; set; }
}
