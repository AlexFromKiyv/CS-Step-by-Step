using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoLot.Models.Entities.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AutoLot.Models.Entities;

[Table("CreditRisks",Schema ="dbo")]
[Index("CustomerId", Name = "IX_CreditRisks_CustomerId")]
[EntityTypeConfiguration(typeof(CreditRiskConfiguration))]
public partial class CreditRisk :BaseEntity
{
    public Person PersonInformation { get; set; } = new Person();
    public int CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty(nameof(Customer.CreditRisks))]
    public virtual Customer CustomerNavigation { get; set; } = null!;
}
