using DataBaseModel.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DataBaseModel.Entity;

[EntityTypeConfiguration(typeof(PlacementtypeConfiguration))]
public partial class Placementtype
{
    [Key]
    public int IdType { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Placement> Placements { get; set; } = new List<Placement>();
}
