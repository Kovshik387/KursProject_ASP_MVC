using DataBaseModel.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DataBaseModel.Entity;

[EntityTypeConfiguration(typeof(AuthorizationTypeConfiguration))]
public partial class Authorizationtype
{
    [Key]
    public int IdType { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<Authorization> Authorizations { get; set; } = new List<Authorization>();
}
