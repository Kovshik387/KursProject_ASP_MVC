using DataBaseModel.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataBaseModel.Entity;
[EntityTypeConfiguration(typeof(UserConfiguration))]
public partial class User
{
    [Key]
    public int IdU { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Sex { get; set; }

    public string? Contact { get; set; }

    public virtual ICollection<Authorization> Authorizations { get; set; } = new List<Authorization>();

    public virtual ICollection<Renter>? Renters { get; set; } = new List<Renter>();

    public virtual ICollection<Tenant>? Tenants { get; set; } = new List<Tenant>();
}
