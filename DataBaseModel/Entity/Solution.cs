using DataBaseModel.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DataBaseModel.Entity;
[EntityTypeConfiguration(typeof(SolutionConfiguration))]
public partial class Solution
{
    [Key]
    public int IdS { get; set; }

    public string? Description { get; set; }

    public DateOnly Datesolution { get; set; }

    public int IdT { get; set; }

    public int IdR { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual Renter IdRNavigation { get; set; } = null!;

    public virtual Tenant IdTNavigation { get; set; } = null!;
}
