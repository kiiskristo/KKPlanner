using System.Collections.ObjectModel;
using System.Windows.Input;
using MAUI.Model;
using MAUI.MVVM;
using MAUI.Services;

namespace MAUI.ViewModel;

public class TermsViewModel : ViewModelBase
{
    public ObservableCollection<Term> Terms { get; } = new ObservableCollection<Term>();
    public ICommand AddCommand { get; private set; }
    public ICommand EditCommand { get; private set; }
    public ICommand EditCourseCommand { get; private set; }
    private readonly ITermService _termService;
    private readonly ICourseService _courseService;
    private readonly INavigationService _navigationService;
    private List<Course> _courses = [];
    
    public TermsViewModel(ITermService termService, ICourseService courseService, INavigationService navigationService)
    {
        _termService = termService;
        _courseService = courseService;
        _navigationService = navigationService;
        AddCommand = new Command(OnAdd);
        EditCommand = new Command<int>(OnEdit);
        EditCourseCommand = new Command<int>(OnEditCourse);
        LoadTerms();
    }

    public async void LoadTerms()
    {
        var termsList = await _termService.GetAllTermsAsync();
        _courses = await _courseService.GetAllCoursesAsync();
        Terms.Clear();
        foreach (var term in termsList)
        {
            term.Courses = _courses.Where(c => c.TermId == term.Id).ToList();
            Terms.Add(term);
        }
    }

    private async void OnEdit(int termId)
    {
        Term? term = Terms.FirstOrDefault(c => c.Id == termId);
        if (term is null) { return; }
        string option = await _navigationService.ActionSheet(term.Title ?? "Term action", ["Edit", "Delete"]);
        Console.WriteLine("Action chosen: " + option);
        switch (option)
        {
            case "Edit":
                _navigationService.NavigateTo($"//terms/detail?termId={termId}");
                break;
            case "Delete":
                DeleteTerm(term);
                break;
        }
    }

    private void DeleteTerm(Term term)
    {
        _termService.DeleteTermAsync(term);
        LoadTerms();
    }

    private async void OnEditCourse(int courseId)
    {
        Course? course = _courses.FirstOrDefault(c => c.Id == courseId);
        if (course is null) { return; }
        string option = await _navigationService.ActionSheet(course.Title ?? "Course action", ["Edit", "Delete"]);
        Console.WriteLine("Action chosen: " + option);
        switch (option)
        {
            case "Edit":
                _navigationService.NavigateTo($"//terms/courseDetail?courseId={courseId}");
                break;
            case "Delete":
                DeleteCourse(course);
                break;
        }
    }
    
    private void DeleteCourse(Course course)
    {
        _courseService.DeleteCourseAsync(course);
        LoadTerms();
    }
    
    private void OnAdd()
    {
        _navigationService.NavigateTo("//terms/detail");
    }

}