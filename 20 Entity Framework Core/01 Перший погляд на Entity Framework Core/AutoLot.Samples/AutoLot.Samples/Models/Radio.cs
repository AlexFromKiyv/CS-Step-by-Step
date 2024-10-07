using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Samples.Models;

public class Radio : BaseEntity
{
    public bool HasTweeters { get; set; }
    public bool HasSubWoofers { get; set; }
    [Required,StringLength(50)]
    public string RadioId { get; set; }
    [Column("InventoryId")]
    public int CarId { get; set; }
    [ForeignKey(nameof(CarId))]
    public Car CarNavigation { get; set; }
}
