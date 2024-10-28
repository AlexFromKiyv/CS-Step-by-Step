using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop;

public class ApplicationDbContext : DbContext
{
    //Properties
    public DbSet<Product> Products { get; set; }


    //Constructors
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
