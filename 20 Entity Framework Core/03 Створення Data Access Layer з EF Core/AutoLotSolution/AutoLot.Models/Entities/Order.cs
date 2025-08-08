namespace AutoLot.Models.Entities;

[Index("CarId", Name = "IX_Orders_CarId")]
[Index("CustomerId", "CarId", Name = "IX_Orders_CustomerId_CarId", IsUnique = true)]
[EntityTypeConfiguration(typeof(OrderConfiguration))]
public partial class Order : BaseEntity
{
    public int CustomerId { get; set; }
    public int CarId { get; set; }
    [ForeignKey(nameof(CarId))]
    [InverseProperty(nameof(Car.Orders))]
    public virtual Car CarNavigation { get; set; } = null!;

    [ForeignKey(nameof(CustomerId))]
    [InverseProperty(nameof(Customer.Orders))]
    public virtual Customer CustomerNavigation { get; set; } = null!;
}