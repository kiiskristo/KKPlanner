using AMMA.Data;
using MAUI.View;
using Syncfusion.Licensing;

namespace MAUI;

public partial class App : Application
{
    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        MainPage = new LoginPage();
        SyncfusionLicenseProvider.RegisterLicense(Constants.SyncfusionKey);
    }
}