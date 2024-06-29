using System.Text.RegularExpressions;

namespace MAUI.Utils.ValidationRules;

public class EmailRule<T> : IValidationRule<string>
{
    public string? ValidationMessage { get; set; }

    public bool Check(string? value)
    {
        if (string.IsNullOrEmpty(value)) return false;
        var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        return regex.IsMatch(value);
    }
}