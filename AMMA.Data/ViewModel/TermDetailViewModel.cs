using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using AMMA.Data.Model;
using AMMA.Data.Services;
using AMMA.Data.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AMMA.Data.ViewModel;

public partial class TermDetailViewModel : ObservableValidator
{
    [ObservableProperty]
    private bool _isEditMode;

    [ObservableProperty]
    private Term _currentTerm;

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private bool _canAddMoreCourses;

    [ObservableProperty] [Required(ErrorMessage = "A Title is required.")]
    private string _title = "";
    
    public ICommand DeleteCommand { get; }
    public ICommand AddCourseCommand { get; }
    public ICommand EditCourseCommand { get; }

    private readonly ITermService _termService;
    private readonly ICourseService _courseService;
    private readonly INavigationUtility _navigationService;
    
    public TermDetailViewModel(ITermService termService, ICourseService courseService, INavigationUtility navigationService)
    {
        _termService = termService;
        _courseService = courseService;
        _navigationService = navigationService;
        CurrentTerm = new Term();
        
        DeleteCommand = new RelayCommand(OnDeleteAsync);
        AddCourseCommand = new RelayCommand(OnCourseAdd);
        EditCourseCommand = new RelayCommand<int>(OnCourseEditAsync);
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
        LoadTermDetailsAsync(CurrentTerm.Id);
    }
    
    private async void LoadTermDetailsAsync(int termId)
    {
        if (termId is 0) { return; }
        
        var term = await _termService.GetTermByIdAsync(termId);
        if (term is not null)
        {
            CurrentTerm = term;
        }

        Title = CurrentTerm.Title ?? "";
        CanAddMoreCourses = CurrentTerm.Courses.Count < 6 && IsEditMode;
        
    }
    
    private void OnCourseAdd()
    {
        _navigationService.OpenModalTo( $"//terms/detail/courseDetail?termId={CurrentTerm.Id}");
    }

    private async void OnCourseEditAsync(int courseId)
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

    [RelayCommand]
    private async Task OnSaveAsync()
    {
        ValidateAllProperties();
        if (!DateRangeIsValid() || HasErrors) { return; }
        CurrentTerm.Title = Title;
        await _termService.SaveTermAsync(CurrentTerm);
        _navigationService.NavigateBack();
    }

    private bool DateRangeIsValid()
    {
        if (CurrentTerm.StartDate >= CurrentTerm.EndDate)
        {
            ErrorMessage = "Start Date must be before End Date.";
            return false;
        }
        return true;
    }

    private async void OnDeleteAsync()
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