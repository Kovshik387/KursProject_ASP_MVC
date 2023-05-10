using DataBaseModel.Configuration;
using DataBaseModel.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DataBaseModel.Entity;
[EntityTypeConfiguration(typeof(ContractConfiguration))]
public partial class Contract
{
    [Key]
    public int IdC { get; set; }

    public int Paymentsize { get; set; }

    public int? IdPay { get; set; }

    public int IdS { get; set; }

    public int IdP { get; set; }

    public virtual Placement IdPNavigation { get; set; } = null!;

    public virtual Payment IdPayNavigation { get; set; } = null!;

    public virtual Solution IdSNavigation { get; set; } = null!;
}
