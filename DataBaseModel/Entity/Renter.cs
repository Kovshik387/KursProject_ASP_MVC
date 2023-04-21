using DataBaseModel.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DataBaseModel.Entity;


[EntityTypeConfiguration(typeof(RenterConfiguration))]
public partial class Renter
{
    [Key]
    public int IdR { get; set; }

    public int License { get; set; }

    public int IdU { get; set; }

    public virtual User IdUNavigation { get; set; } = null!;

    public virtual ICollection<Placement> Placements { get; set; } = new List<Placement>();

    public virtual ICollection<Solution> Solutions { get; set; } = new List<Solution>();
}
