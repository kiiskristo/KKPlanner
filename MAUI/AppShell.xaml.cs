using MAUI.View;
using Plugin.LocalNotification;

namespace MAUI;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
		Routing.RegisterRoute("terms/detail", typeof(TermDetailPage)); //term adding/editing
		Routing.RegisterRoute("terms/detail/courseDetail", typeof(CourseDetailPage)); //adding a course in term detail
		Routing.RegisterRoute("terms/detail/courseDetail/assessment", typeof(AssessmentDetailPage)); //editing term, editing course, editing assessment
		Routing.RegisterRoute("terms/courseDetail", typeof(CourseDetailPage)); //term edit course
		Routing.RegisterRoute("terms/courseDetail/assessment", typeof(AssessmentDetailPage)); //term edit/add course/assessment
		Routing.RegisterRoute("courses/detail", typeof(CourseDetailPage)); //course edit page
		Routing.RegisterRoute("courses/detail/assessment", typeof(AssessmentDetailPage)); //course edit page
		Routing.RegisterRoute("instructors/detail", typeof(InstructorDetailPage));
	}
}
