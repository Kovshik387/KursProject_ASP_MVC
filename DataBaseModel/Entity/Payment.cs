using DataBaseModel.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DataBaseModel.Entity;

[EntityTypeConfiguration(typeof(PaymentConfiguration))]
public partial class Payment
{
    [Key]
    public int IdPay { get; set; }

    public string? Paymentmethod { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}
