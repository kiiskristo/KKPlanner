using System.Collections.ObjectModel;
using System.Windows.Input;
using MAUI.Model;
using MAUI.MVVM;
using MAUI.Services;

namespace MAUI.ViewModel;

public class InstructorsViewModel : ViewModelBase
{
    public ObservableCollection<Instructor> Instructors { get; } = new ObservableCollection<Instructor>();
    
    public ICommand AddCommand { get; private set; }
    public ICommand EditCommand { get; private set; }
    
    private readonly IInstructorService _instructorsService;
    private readonly INavigationService _navigationService;
    
    public InstructorsViewModel(IInstructorService instructorsService, INavigationService navigationService)
    {
        _instructorsService = instructorsService;
        _navigationService = navigationService;
        AddCommand = new Command(OnAdd);
        EditCommand = new Command<int>(OnEdit);
        LoadInstructors();
    }

    public async void LoadInstructors()
    {
        var instructorsList = await _instructorsService.GetAllInstructorsAsync();
        Instructors.Clear();
        foreach (var instructor in instructorsList)
        {
            Instructors.Add(instructor);
        }
    }
    private void OnEdit(int instructorId)
    {
        _navigationService.NavigateTo($"//instructors/detail?instructorId={instructorId}");
    }
    
    private void OnAdd()
    {
        _navigationService.NavigateTo("//instructors/detail");
    }
}