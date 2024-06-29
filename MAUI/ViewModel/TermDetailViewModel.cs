using System.Windows.Input;
using MAUI.Model;
using MAUI.MVVM;
using MAUI.Services;
using MAUI.Utils;
using MAUI.Utils.ValidationRules;
using Microsoft.Maui.Layouts;

namespace MAUI.ViewModel;

public class TermDetailViewModel : ViewModelBase, IQueryAttributable
{
    public ValidatableObject<string> Title { get; } = new ValidatableObject<string>();
    public string? ErrorMessage
    {
        get => _errorMessage;
        private set => SetProperty(ref _errorMessage, value);
    }
    public bool IsEditMode 
    {
        get => _isEditMode;
        private set => SetProperty(ref _isEditMode, value);
    }
    public Term CurrentTerm
    {
        get => _currentTerm;
        private set => SetProperty(ref _currentTerm, value);
    }
    public bool CanAddMoreCourses 
    {
        get => _canAddMoreCourses;
        private set => SetProperty(ref _canAddMoreCourses, value);
    }
    
    private bool _isEditMode;
    private Term _currentTerm;
    private string? _errorMessage;
    private bool _canAddMoreCourses;
    
    public ICommand SaveCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }
    public ICommand AddCourseCommand { get; private set; }
    public ICommand EditCourseCommand { get; private set; }

    private readonly ITermService _termService;
    private readonly ICourseService _courseService;
    private readonly INavigationService _navigationService;
    
    public TermDetailViewModel(ITermService termService, ICourseService courseService, INavigationService navigationService)
    {
        _termService = termService;
        _courseService = courseService;
        _navigationService = navigationService;
        _currentTerm = new Term(); 
        SaveCommand = new Command(OnSave);
        DeleteCommand = new Command(OnDelete);
        AddCourseCommand = new Command(OnCourseAdd);
        EditCourseCommand = new Command<int>(OnCourseEdit);
        Title.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A Title is required." });
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("termId", out var idObj) && idObj is string idString && int.TryParse(idString, out var termId))
        {
            IsEditMode = true;
            LoadTermDetailsAsync(termId);
        }
    }

    public void RefreshDetails()
    {
        LoadTermDetailsAsync(_currentTerm.Id);
    }
    
    private async void LoadTermDetailsAsync(int termId)
    {
        var term = await _termService.GetTermByIdAsync(termId);
        if (term is not null) { CurrentTerm = term; }
        Title.Value = CurrentTerm.Title;
        CanAddMoreCourses = CurrentTerm.Courses.Count < 6 && IsEditMode;
    }
    
    private void OnSave()
    {
        Task.Run(async () => await SaveTermAsync());
    }

    private void OnDelete()
    {
        Task.Run(async () => await DeleteTermAsync());
    }
    
    private void OnCourseAdd()
    {
        _navigationService.OpenModalTo( $"//terms/detail/courseDetail?termId={_currentTerm.Id}");
    }

    private async void OnCourseEdit(int courseId)
    {
        var course = CurrentTerm.Courses.FirstOrDefault(c => c.Id == courseId);
        if (course is null) { return; }
        var option = await _navigationService.ActionSheet(course.Title ?? "Course action", ["Edit", "Delete"]);
        switch (option)
        {
            case "Edit":
                _navigationService.NavigateTo($"//terms/detail/courseDetail?courseId={courseId}");
                break;
            case "Delete":
                await DeleteCourseAsync(course);
                break;
        }
    }

    private async Task SaveTermAsync()
    {
        if (!Title.Validate() || !DateValidate()) { return; }
        CurrentTerm.Title = Title.Value;
        await _termService.SaveTermAsync(CurrentTerm);
        _navigationService.NavigateBack();
    }

    private bool DateValidate()
    {
        if (CurrentTerm.StartDate >= CurrentTerm.EndDate)
        {
            ErrorMessage = "Start Date must be before End Date.";
            OnPropertyChanged(nameof(ErrorMessage));
            return false;
        }
        ErrorMessage = null;
        OnPropertyChanged(nameof(ErrorMessage));
        return true;
    }

    private async Task DeleteTermAsync()
    {
        await _termService.DeleteTermAsync(CurrentTerm);
        _navigationService.NavigateBack();
    }
    private async Task DeleteCourseAsync(Course course)
    {
        await _courseService.DeleteCourseAsync(course);
        RefreshDetails();
    }
}