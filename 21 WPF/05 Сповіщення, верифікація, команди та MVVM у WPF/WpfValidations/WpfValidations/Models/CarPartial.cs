using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WpfValidations.Models;

public partial class Car : BaseEntity, IDataErrorInfo
{
    public string Error { get; }

    //public string this[string columnName]
    //{
    //    get
    //    {
    //        switch (columnName)
    //        {
    //            case nameof(Id):
    //                break;
    //            case nameof(Make):
    //                return Make == "ModelT" ? "Too Old" : CheckMakeAndColor();
    //            case nameof(Color):
    //                return CheckMakeAndColor();
    //            case nameof(PetName):
    //                break;
    //        }
    //        return string.Empty;
    //    }
    //}

    //private string CheckMakeAndColor()
    //{
    //    if (Make == "Chevy" && Color == "Pink")
    //    {
    //        return $"{Make}'s don't come in {Color}";
    //    }
    //    return string.Empty;
    //}
    //public string Error { get; }

    //private readonly Dictionary<string, List<string>> _errors = new();
    //public bool HasErrors => _errors.Any();

    //public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    //public IEnumerable GetErrors(string? propertyName)
    //{
    //    if (string.IsNullOrEmpty(propertyName))
    //    {
    //        return _errors.Values;
    //    }
    //    return _errors.ContainsKey(propertyName) ? _errors[propertyName] : null;
    //}

    //private void AddError(string propertyName, string error)
    //{
    //    AddErrors(propertyName, new List<string> { error });
    //}
    //private void AddErrors(string propertyName, IList<string> errors)
    //{
    //    if (errors == null || !errors.Any())
    //    {
    //        return;
    //    }
    //    var changed = false;
    //    if (!_errors.ContainsKey(propertyName))
    //    {
    //        _errors.Add(propertyName, new List<string>());
    //        changed = true;
    //    }
    //    foreach (var err in errors)
    //    {
    //        if (_errors[propertyName].Contains(err)) continue;
    //        _errors[propertyName].Add(err);
    //        changed = true;
    //    }
    //    if (changed)
    //    {
    //        OnErrorsChanged(propertyName);
    //    }
    //}
    //protected void ClearErrors(string propertyName = "")
    //{
    //    if (string.IsNullOrEmpty(propertyName))
    //    {
    //        _errors.Clear();
    //    }
    //    else
    //    {
    //        _errors.Remove(propertyName);
    //    }
    //    OnErrorsChanged(propertyName);
    //}


    //private void OnErrorsChanged(string propertyName)
    //{
    //    ErrorsChanged?.Invoke(this,
    //      new DataErrorsChangedEventArgs(propertyName));
    //}

    public string this[string columnName]
    {
        get
        {
            ClearErrors(columnName);
            var errorsFromAnnotations = GetErrorsFromAnnotations(columnName,
                typeof(Car).GetProperty(columnName)?.GetValue(this, null));
            if (errorsFromAnnotations != null)
            {
                AddErrors(columnName, errorsFromAnnotations);
            }
            switch (columnName)
            {
                case nameof(Id):
                    break;
                case nameof(Make):
                    CheckMakeAndColor();
                    if (Make == "ModelT")
                    {
                        AddError(nameof(Make), "Too Old");
                    }
                    break;
                case nameof(Color):
                    CheckMakeAndColor();
                    break;
                case nameof(PetName):
                    break;
            }
            return string.Empty;
        }
    }
    internal bool CheckMakeAndColor()
    {
        if (Make == "Chevy" && Color == "Pink")
        {
            AddError(nameof(Make), $"{Make}'s don't come in {Color}");
            AddError(nameof(Color),
              $"{Make}'s don't come in {Color}");
            return true;
        }
        return false;
    }
}
