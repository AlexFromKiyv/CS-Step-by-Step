﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Models.Entities.Owned;

[Owned]
public class Person
{
    [Required, StringLength(50)]
    public string FirstName { get; set; }
    [Required, StringLength(50)]
    public string LastName { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string FullName { get; set; }
}