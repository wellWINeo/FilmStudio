using System;
using System.Linq;
using System.Collections.Generic;
using FilmStudio.Models;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace FilmStudio.Helpers;


public class EmployeesConverter : IBindingTypeConverter
{
    public int GetAffinityForObjects(Type fromType, Type toType) => 100;

    public bool TryConvert(object? from, Type toType, object? conversionHint, out object? result)
    {
        result = from;
        return true;
    }
}