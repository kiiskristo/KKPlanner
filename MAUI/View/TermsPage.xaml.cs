using System.ComponentModel;
using CommunityToolkit.Maui.Views;
using MAUI.Model;
using MAUI.Utils;
using MAUI.ViewModel;
using Plugin.LocalNotification;

namespace MAUI.View;

public partial class TermsPage : ContentPage
{
    private readonly INotificationUtility _notificationUtility;
    public TermsPage(INotificationUtility notificationUtility, TermsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _notificationUtility = notificationUtility;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((TermsViewModel)BindingContext).LoadTerms();
        InitializeNotifications();
    }
    
    private async void InitializeNotifications()
    {
        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }
        await _notificationUtility.CheckAndNotifyAsync();
    }
}
