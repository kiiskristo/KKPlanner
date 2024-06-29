using System.Collections.ObjectModel;
using AMMA.Data.Model;
using AMMA.Data.Services;
using AMMA.Data.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AMMA.Data.ViewModel;

public partial class TermsViewModel : ObservableObject
{
    private readonly ITermService _termService;
    private readonly ICourseService _courseService;
    private readonly INavigationUtility _navigationService;

    [ObservableProperty]
    private ObservableCollection<Term> _terms = [];

    public TermsViewModel(ITermService termService, ICourseService courseService, INavigationUtility navigationService)
    {
        _termService = termService;
        _courseService = courseService;
        _navigationService = navigationService;
        LoadTermsAsync();
    }
    
    public async void LoadTermsAsync()
    {
        var termsList = await _termService.GetAllTermsAsync();
        Terms.Clear();

        foreach (var term in termsList)
        {
            Terms.Add(term);
        }
    }

    [RelayCommand]
    private async Task OnEditTerm(int termId)
    {
        var term = await _termService.GetTermByIdAsync(termId);
        if (term == null) return;

        var option = await _navigationService.ActionSheet(term.Title ?? "Term action", ["Edit", "Delete"]);

        switch (option)
        {
            case "Edit":
                _navigationService.NavigateTo($"//terms/detail?termId={termId}");
                break;
            case "Delete":
                await DeleteTerm(term);
                break;
        }
    }

    private async Task DeleteTerm(Term term)
    {
        await _termService.DeleteTermAsync(term);
        LoadTermsAsync();
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
                _navigationService.NavigateTo($"//terms/courseDetail?courseId={courseId}");
                break;
            case "Delete":
                await DeleteCourse(course);
                break;
        }
    }
    
    private async Task DeleteCourse(Course course)
    {
        await _courseService.DeleteCourseAsync(course);
        LoadTermsAsync();
    }

    [RelayCommand]
    private void OnAdd()
    {
        _navigationService.NavigateTo("//terms/detail");
    }
}
