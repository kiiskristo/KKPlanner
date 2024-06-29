using System.Collections.ObjectModel;
using System.Windows.Input;
using MAUI.Model;
using MAUI.MVVM;
using MAUI.Services;

namespace MAUI.ViewModel;

public class CoursesViewModel : ViewModelBase
{
    public ObservableCollection<Course> Courses { get; } = new ObservableCollection<Course>();
    public ICommand AddCommand { get; private set; }
    public ICommand EditCommand { get; private set; }
    private readonly ICourseService _courseService;
    private readonly INavigationService _navigationService;
    
    public CoursesViewModel(ICourseService courseService, INavigationService navigationService)
    {
        _courseService = courseService;
        _navigationService = navigationService;
        AddCommand = new Command(OnAdd);
        EditCommand = new Command<Course>(OnEdit);
        LoadCourses();
    }

    public async void LoadCourses()
    {
        var coursesList = await _courseService.GetAllCoursesAsync();
        Courses.Clear();
        foreach (var course in coursesList)
        {
            Courses.Add(course);
        }
    }
    private void OnEdit(Course course)
    {
        _navigationService.NavigateTo($"//courses/detail?courseId={course.Id}");
    }
    
    private void OnAdd()
    {
        _navigationService.NavigateTo("//courses/detail");
    }
}