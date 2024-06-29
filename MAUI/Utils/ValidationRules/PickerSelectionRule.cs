namespace MAUI.Utils.ValidationRules;

public class PickerSelectionRule<T> : IValidationRule<int>
{
    public string? ValidationMessage { get; set; }

    public bool Check(int value)
    {
        return value != -1;
    }
}