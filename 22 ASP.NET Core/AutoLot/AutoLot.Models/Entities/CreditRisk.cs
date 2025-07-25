﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AutoLot.Models.Entities;

[Index("CustomerId", Name = "IX_CreditRisks_CustomerId")]
[EntityTypeConfiguration(typeof(CreditRiskConfiguration))]
public partial class CreditRisk : BaseEntity
{
    public Person PersonInformation { get; set; } = new Person();
    public int CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    [InverseProperty(nameof(Customer.CreditRisks))]
    public virtual Customer CustomerNavigation { get; set; } = null!;
}