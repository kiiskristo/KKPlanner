using CommunityToolkit.Maui;
using Plugin.LocalNotification;
using MAUI.Services;
using MAUI.Utils;
using MAUI.View;
using MAUI.ViewModel;
using Microsoft.Extensions.Logging;

namespace MAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("fa-solid-900.ttf", "FASolid");
			})
			.UseMauiCommunityToolkit()
			.UseLocalNotification();
		
		RegisterServices(builder);
		RegisterViewModels(builder);
		RegisterPages(builder);

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	private static void RegisterServices(MauiAppBuilder builder)
	{
		builder.Services.AddSingleton<ITermService, TermDatabaseService>();
		builder.Services.AddSingleton<ICourseService, CourseDatabaseService>();
		builder.Services.AddSingleton<IInstructorService, InstructorDatabaseService>();
		builder.Services.AddSingleton<IAssessmentService, AssessmentDatabaseService>();
		builder.Services.AddSingleton<INavigationService, NavigationService>();
		builder.Services.AddSingleton<INotificationUtility, NotificationUtility>();
	}
	
	private static void RegisterViewModels(MauiAppBuilder builder)
	{
		builder.Services.AddTransient<TermsViewModel>();
		builder.Services.AddTransient<TermDetailViewModel>();
		builder.Services.AddTransient<CoursesViewModel>();
		builder.Services.AddTransient<CourseDetailViewModel>();
		builder.Services.AddTransient<InstructorsViewModel>();
		builder.Services.AddTransient<InstructorDetailViewModel>();
		builder.Services.AddTransient<AssessmentDetailViewModel>();
	}
	
	private static void RegisterPages(MauiAppBuilder builder)
	{
		builder.Services.AddTransient<TermsPage>();
		builder.Services.AddTransient<TermDetailPage>();
		builder.Services.AddTransient<CoursesPage>();
		builder.Services.AddTransient<CourseDetailPage>();
		builder.Services.AddTransient<InstructorsPage>();
		builder.Services.AddTransient<InstructorDetailPage>();
		builder.Services.AddTransient<AssessmentDetailPage>();
	}
}