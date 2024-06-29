using System.Collections.ObjectModel;
using System.Windows.Input;
using AMMA.Data.Model;
using AMMA.Data.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI.Services;

namespace AMMA.Data.ViewModel;

public class InstructorsViewModel : ObservableObject
{
    public ObservableCollection<Instructor> Instructors { get; } = new ObservableCollection<Instructor>();
    
    public ICommand AddCommand { get; private set; }
    public ICommand EditCommand { get; private set; }
    
    private readonly IInstructorService _instructorsService;
    private readonly INavigationUtility _navigationService;
    
    public InstructorsViewModel(IInstructorService instructorsService, INavigationUtility navigationService)
    {
        _instructorsService = instructorsService;
        _navigationService = navigationService;
        AddCommand = new RelayCommand(OnAdd);
        EditCommand = new RelayCommand<int>(OnEdit);
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