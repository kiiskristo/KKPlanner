namespace AMMA.Data.Utils.ValidationRules;

public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
{
    public string? ValidationMessage { get; set; }

    public bool Check(T? value)
    {
        if (value is string stringValue)
        {
            return !string.IsNullOrWhiteSpace(stringValue);
        }
        return value != null;
    }
}