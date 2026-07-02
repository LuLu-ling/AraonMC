using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace AraonMC.Converters;

/// <summary>Converts a boolean (account online state) to a status label.</summary>
public sealed class BoolToStatusConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is true ? "Online" : "Offline";

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
