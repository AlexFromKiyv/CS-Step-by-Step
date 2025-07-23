using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Dal.Repos;

public class RadioRepo : TemporalTableBaseRepo<Radio>, IRadioRepo
{
    public RadioRepo(ApplicationDbContext context) : base(context)
    {
    }

    public RadioRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}