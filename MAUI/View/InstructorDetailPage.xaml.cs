using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAUI.ViewModel;

namespace MAUI.View;

public partial class InstructorDetailPage : ContentPage
{
    public InstructorDetailPage(InstructorDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}