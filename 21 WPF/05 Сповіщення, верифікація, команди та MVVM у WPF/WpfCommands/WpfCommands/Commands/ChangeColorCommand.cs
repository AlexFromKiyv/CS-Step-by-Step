using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfCommands.Models;

namespace WpfCommands.Commands;

public class ChangeColorCommand : CommandBase
{
    //public event EventHandler? CanExecuteChanged
    //{
    //    add => CommandManager.RequerySuggested += value;
    //    remove => CommandManager.RequerySuggested -= value;
    //}

    public override bool CanExecute(object? parameter) => (parameter as Car) != null;
    
    public override void Execute(object? parameter)
    {
        ((Car)parameter).Color = "Pink";
    }
}
