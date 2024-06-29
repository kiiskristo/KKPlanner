using System.Windows.Input;
using AMMA.Data.Model;
using AMMA.Data.Utils;
using AMMA.Data.Utils.ValidationRules;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI.Services;

namespace AMMA.Data.ViewModel;

public class InstructorDetailViewModel : ObservableObject
{
    public ValidatableObject<string> Name { get; } = new ValidatableObject<string>();
    public ValidatableObject<string> Email { get; } = new ValidatableObject<string>();
    public ValidatableObject<string> Phone { get; } = new ValidatableObject<string>();
    public bool IsEditMode 
    {
        get => _isEditMode;
        private set => SetProperty(ref _isEditMode, value);
    }
    public Instructor CurrentInstructor
    {
        get => _currentInstructor;
        private set => SetProperty(ref _currentInstructor, value);
    }
    public ICommand SaveCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }
    
    private bool _isEditMode;
    private Instructor _currentInstructor;
    
    private readonly IInstructorService _instructorService;
    private readonly INavigationUtility _navigationService;
    
    public InstructorDetailViewModel(
        IInstructorService instructorService, 
        INavigationUtility navigationService)
    {
        _instructorService = instructorService;
        _navigationService = navigationService;
        _currentInstructor = new Instructor(); 
        
        Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A name is required." });
        Email.Validations.Add(new EmailRule<string> { ValidationMessage = "A valid e-mail must be set." });
        Phone.Validations.Add(new PhoneRule<string> { ValidationMessage = "A valid phone must be set." });
        
        SaveCommand = new RelayCommand(OnSave);
        DeleteCommand = new RelayCommand(OnDelete);
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("instructorId", out var idObj) && idObj is string idString && int.TryParse(idString, out var instructorId))
        {
            IsEditMode = true;
            LoadInstructorDetailsAsync(instructorId);
        }
    }
    
    private async void LoadInstructorDetailsAsync(int instructorId)
    {
        CurrentInstructor = await _instructorService.GetInstructorByIdAsync(instructorId);
        Name.Value = CurrentInstructor.Name;
        Email.Value = CurrentInstructor.Email;
        Phone.Value = CurrentInstructor.PhoneNumber;
    }
    
    private void OnSave()
    {
        if (!Name.Validate() || !Email.Validate() || !Phone.Validate()) { return; }
        
        CurrentInstructor.Name = Name.Value;
        CurrentInstructor.Email = Email.Value;
        CurrentInstructor.PhoneNumber = Phone.Value;
        
        Task.Run(async () => await SaveInstructorAsync());
        
    }

    private void OnDelete()
    {
        Task.Run(async () => await DeleteInstructorAsync());
    }

    private async Task SaveInstructorAsync()
    {
        await _instructorService.SaveInstructorAsync(CurrentInstructor);
        _navigationService.NavigateBack();
    }

    private async Task DeleteInstructorAsync()
    {
        await _instructorService.DeleteInstructorAsync(CurrentInstructor);
        _navigationService.NavigateBack();
    }
}