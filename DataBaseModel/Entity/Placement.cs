using DataBaseModel.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DataBaseModel.Entity;

[EntityTypeConfiguration(typeof(PlacementConfiguration))]
public partial class Placement
{
    [Key]
    public int IdP { get; set; }

    public int Floor { get; set; }

    public string Square { get; set; } = null!;

    public int Room { get; set; }

    public string Area { get; set; } = null!;

    public string Street { get; set; } = null!;

    public int Number { get; set; }

    public int IdType { get; set; }

    public int IdR { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual Renter IdRNavigation { get; set; } = null!;

    public virtual Placementtype IdTypeNavigation { get; set; } = null!;
}
