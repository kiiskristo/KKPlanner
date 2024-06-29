using System.Text.RegularExpressions;

namespace MAUI.Utils.ValidationRules;

public class PhoneRule<T> : IValidationRule<string>
{
    public string? ValidationMessage { get; set; }

    public bool Check(string? value)
    {
        if (string.IsNullOrEmpty(value)) return false;
        var regex = new Regex(@"^\+?\d{1,3}?[-.\s]?\(?\d{1,3}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9}$");
        return regex.IsMatch(value);
    }
}