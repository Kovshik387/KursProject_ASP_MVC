using DataBaseModel.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DataBaseModel.Entity;

[EntityTypeConfiguration(typeof(AuthorizationConfiguration))]
public partial class Authorization
{
    [Key]
    public int IdA { get; set; }

    public string Loginuser { get; set; } = null!;

    public string Passworduser { get; set; } = null!;

    public int? IdU { get; set; }

    public int? IdType { get; set; }

    public virtual Authorizationtype? IdTypeNavigation { get; set; }

    public virtual User? IdUNavigation { get; set; }
}
