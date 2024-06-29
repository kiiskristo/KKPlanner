using CommunityToolkit.Mvvm.ComponentModel;

namespace AMMA.Data.Utils;

public class ValidatableObject<T> : ObservableObject
{
    private readonly List<string> _errors = [];
    private T? _value;

    public string ErrorString => string.Join("\n", _errors);

    public bool HasErrors => _errors.Any();

    public T? Value
    {
        get => _value;
        set
        {
            if (SetProperty(ref _value, value))
            {
                Validate();
            }
        }
    }

    public List<IValidationRule<T>> Validations { get; } = [];

    public bool Validate()
    {
        _errors.Clear();
        foreach (var validation in Validations)
        {
            if (!validation.Check(Value))
            {
                _errors.Add(validation.ValidationMessage ?? "Unknown error");
            }
        }

        // Notifying changes for ErrorString and HasErrors to update the UI
        OnPropertyChanged(nameof(ErrorString));
        OnPropertyChanged(nameof(HasErrors));

        return !HasErrors;
    }
}