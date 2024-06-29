using System.Windows.Input;
using MAUI.Model;
using MAUI.MVVM;
using MAUI.Services;
using MAUI.Utils;
using MAUI.Utils.ValidationRules;

namespace MAUI.ViewModel;

public class AssessmentDetailViewModel : ViewModelBase, IQueryAttributable
{
    public string? ErrorMessage
    {
        get => _errorMessage;
        private set => SetProperty(ref _errorMessage, value);
    }
    public ValidatableObject<string> Name { get; } = new ValidatableObject<string>();
    public ValidatableObject<string> Type { get; } = new ValidatableObject<string>();
    
    public Assessment CurrentAssessment
    {
        get => _currentAssessment;
        private set => SetProperty(ref _currentAssessment, value);
    }
    
    public List<string> Types { get; } = ["Performance", "Objective"];
    public ICommand SaveCommand { get; private set; }
    public ICommand CloseCommand { get; private set; }
    
    private string? _errorMessage;
    private Assessment _currentAssessment;
    
    private readonly IAssessmentService _assessmentService;
    private readonly INavigationService _navigationService;
    
    public AssessmentDetailViewModel(
        IAssessmentService assessmentService, 
        INavigationService navigationService)
    {
        _assessmentService = assessmentService;
        _navigationService = navigationService;
        _currentAssessment = new Assessment(); 
        
        Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A name is required." });
        Type.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Assessment type is required" });
        
        SaveCommand = new Command(OnSave);
        CloseCommand = new Command(OnClose);
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("courseId", out var courseIdObj) && courseIdObj is string courseIdString && int.TryParse(courseIdString, out var courseId))
        {
            CurrentAssessment.CourseId = courseId;
        }
        if (query.TryGetValue("assessmentId", out var idObj) && idObj is string idString && int.TryParse(idString, out var assessmentId))
        {
            LoadAssessmentDetailsAsync(assessmentId);
        }
    }
    
    private async void LoadAssessmentDetailsAsync(int assessmentId)
    {
        CurrentAssessment = await _assessmentService.GetAssessmentByIdAsync(assessmentId);
        Name.Value = CurrentAssessment.Name;
        Type.Value = CurrentAssessment.Type;
    }
    
    private void OnSave()
    {
        if (!Name.Validate() || !DateValidate() || !Type.Validate()) { return; }
        CurrentAssessment.Name = Name.Value;
        CurrentAssessment.Type = Type.Value;
        Task.Run(async () => await SaveAssessmentAsync());
        _navigationService.NavigateBack();
    }

    private void OnClose()
    {
        _navigationService.NavigateBack();
    }

    private async Task SaveAssessmentAsync()
    {
        await _assessmentService.SaveAssessmentAsync(CurrentAssessment);
    }
    
    private bool DateValidate()
    {
        if (CurrentAssessment.StartDate >= CurrentAssessment.EndDate)
        {
            ErrorMessage = "Start Date must be before End Date.";
            OnPropertyChanged(nameof(ErrorMessage));
            return false;
        }
        ErrorMessage = null;
        OnPropertyChanged(nameof(ErrorMessage));
        return true;
    }
}