using System;
using System.Collections.Generic;

namespace GiftStore.Models;

public partial class Departmentemployee
{
    public string Departmentname { get; set; } = null!;

    public decimal Employeeid { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public decimal? Salary { get; set; }

    public string? Phonenumber { get; set; }

    public string? Address { get; set; }
}
