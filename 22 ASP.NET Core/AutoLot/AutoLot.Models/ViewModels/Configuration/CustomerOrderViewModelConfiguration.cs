using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Models.ViewModels.Configuration;

public class CustomerOrderViewModelConfiguration : IEntityTypeConfiguration<CustomerOrderViewModel>
{
    public void Configure(EntityTypeBuilder<CustomerOrderViewModel> builder)
    {
        builder.ToView("CustomerOrderView");
    }
}
