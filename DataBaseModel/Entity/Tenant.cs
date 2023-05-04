using DataBaseModel.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DataBaseModel.Entity;
[EntityTypeConfiguration(typeof(TenantConfiguration))]
public partial class Tenant : IEntityType
{
    [Key]
    public int IdT { get; set; }

    public int Rating { get; set; }

    public int IdU { get; set; }

    public virtual User IdUNavigation { get; set; } = null!;

    public virtual ICollection<Solution> Solutions { get; set; } = new List<Solution>();
}
