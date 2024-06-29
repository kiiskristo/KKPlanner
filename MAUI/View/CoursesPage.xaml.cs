using AMMA.Data.ViewModel;
using Microsoft.Maui.Controls;

namespace MAUI.View;

public partial class CoursesPage : ContentPage
{
    public CoursesPage(CoursesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((CoursesViewModel)BindingContext).LoadCourses();
    }
    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (sender is ListView listView)
        {
            listView.SelectedItem = null;
        }
    }
}