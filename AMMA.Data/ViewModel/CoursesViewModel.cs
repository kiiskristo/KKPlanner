using System.Collections.ObjectModel;
using AMMA.Data.Model;
using AMMA.Data.Services;
using AMMA.Data.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AMMA.Data.ViewModel;

public partial class CoursesViewModel : ObservableObject
{
    [ObservableProperty]
    private string _searchQuery = string.Empty;
    [ObservableProperty]
    private ObservableCollection<Course> _courses = [];
    private readonly ObservableCollection<Course> _allCourses = [];
    
    private readonly ICourseService _courseService;
    private readonly INavigationUtility _navigationService;
    
    
    public CoursesViewModel(ICourseService courseService, INavigationUtility navigationService)
    {
        _courseService = courseService;
        _navigationService = navigationService;
        LoadCourses();
    }

    public async void LoadCourses()
    {
        var coursesList = await _courseService.GetAllCoursesAsync();
        _allCourses.Clear();
        foreach (var course in coursesList)
        {
            _allCourses.Add(course);
        }
        FilterCourses();
    }
    
    partial void OnSearchQueryChanged(string value)
    {
        FilterCourses();
    }
    private async void FilterCourses()
    {
        var filteredCourses = await Task.Run(() =>
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                return new ObservableCollection<Course>(_allCourses);
            }
            else
            {
                return new ObservableCollection<Course>(_allCourses.Where(c => c.Title?.ToLower().Contains(SearchQuery.ToLower()) ?? false));
            }
        });
        //be sure main thread...
        Courses = filteredCourses;
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        _navigationService.NavigateTo("//courses/detail");
    }
    
    [RelayCommand]
    private async Task OnEditCourse(int courseId)
    {
        var course = await _courseService.GetCourseByIdAsync(courseId);
        if (course == null) return;

        var option = await _navigationService.ActionSheet(course.Title ?? "Course action", ["Edit", "Delete"]);

        switch (option)
        {
            case "Edit":
                _navigationService.NavigateTo($"//courses/detail?courseId={courseId}");
                break;
            case "Delete":
                await DeleteCourse(course);
                break;
        }
    }
    
    private async Task DeleteCourse(Course course)
    {
        await _courseService.DeleteCourseAsync(course);
        LoadCourses();
    }
}