using System.Collections.ObjectModel;
using System.Windows.Input;
using MAUI.Model;
using MAUI.MVVM;
using MAUI.Services;
using MAUI.Utils;
using MAUI.Utils.ValidationRules;

namespace MAUI.ViewModel;

public class CourseDetailViewModel : ViewModelBase, IQueryAttributable
{
    public ValidatableObject<string> Title { get; } = new ValidatableObject<string>();
    public ValidatableObject<string> Status { get; } = new ValidatableObject<string>();
    public ValidatableObject<Term> SelectedTerm { get; } = new ValidatableObject<Term>();
    public ValidatableObject<string> Name { get; } = new ValidatableObject<string>();
    public ValidatableObject<string> Email { get; } = new ValidatableObject<string>();
    public ValidatableObject<string> Phone { get; } = new ValidatableObject<string>();
    public bool IsEditMode 
    {
        get => _isEditMode;
        private set => SetProperty(ref _isEditMode, value);
    }
    public bool CanAddMoreAssessments 
    {
        get => _canAddMoreAssessments;
        private set => SetProperty(ref _canAddMoreAssessments, value);
    }
    public Course CurrentCourse
    {
        get => _currentCourse;
        private set => SetProperty(ref _currentCourse, value);
    }
    public string? ErrorMessage
    {
        get => _errorMessage;
        private set => SetProperty(ref _errorMessage, value);
    }
    
    public ICommand SaveCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }
    public ICommand AddAssessmentCommand { get; private set; }
    public ICommand EditAssessmentCommand { get; private set; }
    public ICommand ShareNotesCommand { get; private set; }
    public ObservableCollection<Term> Terms { get; } = [];
    private ObservableCollection<Instructor> Instructors { get; } = [];
    public List<string> Statuses { get; } = 
    [
        "In Progress",
        "Completed",
        "Dropped",
        "Plan to Take"
    ];
    
    private bool _isEditMode;
    private bool _canAddMoreAssessments;
    private Course _currentCourse;
    private int? _coursePickerTermId;
    private string? _errorMessage;
    
    private readonly ICourseService _courseService;
    private readonly ITermService _termService;
    private readonly IInstructorService _instructorService;
    private readonly IAssessmentService _assessmentService;
    private readonly INavigationService _navigationService;
    
    public CourseDetailViewModel(
        ICourseService courseService, 
        ITermService termService,
        IInstructorService instructorService,
        IAssessmentService assessmentService,
        INavigationService navigationService
        )
    {
        _courseService = courseService;
        _termService = termService;
        _instructorService = instructorService;
        _assessmentService = assessmentService;
        _navigationService = navigationService;
        _currentCourse = new Course(); 
        SaveCommand = new Command(OnSave);
        DeleteCommand = new Command(OnDelete);
        AddAssessmentCommand = new Command(OnAssessmentAdd);
        EditAssessmentCommand = new Command<int>(OnAssessmentEdit);
        ShareNotesCommand = new Command(ShareNotes);
        
        Title.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A name is required." });
        Status.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Status is required" });
        SelectedTerm.Validations.Add(new IsNotNullOrEmptyRule<Term> { ValidationMessage = "Term is required." });
        Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A instructor name is required." });
        Email.Validations.Add(new EmailRule<string> { ValidationMessage = "A instructor valid e-mail must be set." });
        Phone.Validations.Add(new PhoneRule<string> { ValidationMessage = "A instructor valid phone must be set." });
        
        LoadAdditionalDataAsync();
    }
    
    private async void LoadAdditionalDataAsync()
    {
        var terms = await _termService.GetAllTermsAsync();
        var instructors = await _instructorService.GetAllInstructorsAsync();

        Terms.Clear();
        Instructors.Clear();

        foreach (var term in terms) Terms.Add(term);
        foreach (var instructor in instructors) Instructors.Add(instructor);
        if (_coursePickerTermId is not null)
        {
            SelectedTerm.Value = Terms.FirstOrDefault(t => t.Id == _coursePickerTermId);
        }
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("courseId", out var idObj) && idObj is string idString && int.TryParse(idString, out var courseId))
        {
            IsEditMode = true;
            LoadCourseDetailsAsync(courseId);
        }
        else
        {
            IsEditMode = false;
        }

        if (query.TryGetValue("termId", out var termIdObj) && termIdObj is string termIdString &&
            int.TryParse(termIdString, out var termId))
        {
            _coursePickerTermId = termId;
            CurrentCourse.TermId = termId;
        }
    }
    
    public void RefreshDetails()
    {
        LoadCourseDetailsAsync();
    }
    
    private async void LoadCourseDetailsAsync(int courseId = 0)
    {
        if (courseId is not 0)
        {
            var course = await _courseService.GetCourseByIdAsync(courseId);
            if (course is not null) { CurrentCourse = course; }
            Title.Value = CurrentCourse.Title;
            Status.Value = CurrentCourse.Status; 
            SelectedTerm.Value = Terms.FirstOrDefault(t => t.Id == CurrentCourse.TermId);
            CanAddMoreAssessments = CurrentCourse.Assessments.Count < 2;
            var instructor = Instructors.FirstOrDefault(i => i.Id == CurrentCourse.InstructorId);
            if (instructor is null) { return; }
            Name.Value = instructor.Name;
            Email.Value = instructor.Email;
            Phone.Value = instructor.PhoneNumber;
        }
    }
    
    private void OnSave()
    {
        if (!Title.Validate() || 
            !DateValidate() || 
            !Status.Validate() || 
            !SelectedTerm.Validate() || 
            !Name.Validate() ||
            !Email.Validate() ||
            !Phone.Validate()) { return; }
        
        CurrentCourse.Title = Title.Value;
        CurrentCourse.Status = Status.Value;
        CurrentCourse.TermId = SelectedTerm.Value?.Id;
        
        Task.Run(async () => await SaveCourseAsync());
        _navigationService.NavigateBack();
    }

    private void OnDelete()
    {
        Task.Run(async () => await DeleteCourseAsync());
        _navigationService.NavigateBack();
    }
    private void OnAssessmentAdd()
    {
        _navigationService.NavigateTo($"assessment?courseId={_currentCourse.Id}", true);
    }
    
    private async void OnAssessmentEdit(int assessmentId)
    {
        var assessment = CurrentCourse.Assessments.FirstOrDefault(c => c.Id == assessmentId);
        if (assessment is null) { return; }
        var option = await _navigationService.ActionSheet(assessment.AssessmentName, ["Edit", "Delete"]);
        switch (option)
        {
            case "Edit":
                _navigationService.NavigateTo($"assessment?assessmentId={assessmentId}", true);
                break;
            case "Delete":
                await DeleteAssessmentAsync(assessment);
                break;
        }
    }
    
    private async Task DeleteAssessmentAsync(Assessment assessment)
    {
        await _assessmentService.DeleteAssessmentAsync(assessment);
        LoadCourseDetailsAsync(CurrentCourse.Id);
    }
    
    private async void ShareNotes()
    {
        if (string.IsNullOrWhiteSpace(CurrentCourse.Notes)) { return; }
        await Share.RequestAsync(new ShareTextRequest
        {
            Text = CurrentCourse.Notes,
            Title = "Share Notes"
        });
    }
    
    private async Task SaveCourseAsync()
    {
        var instructorId = await SaveInstructorAsync();
        CurrentCourse.InstructorId = instructorId;
        await _courseService.SaveCourseAsync(CurrentCourse);
    }

    private async Task<int> SaveInstructorAsync()
    {
        var instructor = new Instructor
        {
            Id = CurrentCourse.InstructorId ?? 0,
            Name = Name.Value,
            Email = Email.Value,
            PhoneNumber = Phone.Value
        };
        await _instructorService.SaveInstructorAsync(instructor);
        return instructor.Id;
    }

    private async Task DeleteCourseAsync()
    {
        await _courseService.DeleteCourseAsync(CurrentCourse);
    }
    
    private bool DateValidate()
    {
        if (CurrentCourse.StartDate >= CurrentCourse.EndDate)
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