using System;
using AMMA.Data.Model;
using AMMA.Data.ViewModel;
using Microsoft.Maui.Controls;

namespace MAUI.View;

public partial class InstructorsPage
{
    public InstructorsPage(InstructorsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((InstructorsViewModel)BindingContext).LoadInstructors();
    }
    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Instructor selectedInstructor)
        {
            Console.WriteLine($"selected instructor: {selectedInstructor.Id}");
            ((InstructorsViewModel)BindingContext).EditCommand.Execute(selectedInstructor.Id);
        }
        if (sender is ListView listView)
        {
            listView.SelectedItem = null;
        }
    }
}