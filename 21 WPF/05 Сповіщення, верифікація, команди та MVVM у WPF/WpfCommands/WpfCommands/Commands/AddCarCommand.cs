using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using WpfCommands.Models;

namespace WpfCommands.Commands;

class AddCarCommand : CommandBase
{
    public override bool CanExecute(object? parameter) =>
        parameter is ObservableCollection<Car>;

    public override void Execute(object? parameter)
    {
        if (parameter is not ObservableCollection<Car> cars)
        {
            return;
        }
        var maxCount = cars.Max(x => x.Id);
        cars.Add(new Car
        {
            Id = ++maxCount,
            Color = "Yellow",
            Make = "VW",
            PetName = "Birdie"
        });
    }
}
