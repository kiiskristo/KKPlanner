using MAUI.Services;
using Plugin.LocalNotification;

namespace MAUI.Utils
{
    public interface INotificationUtility
    {
        Task CheckAndNotifyAsync();
    }
    
    public class NotificationUtility: INotificationUtility
    {
        private readonly ICourseService _courseService;
        private readonly IAssessmentService _assessmentService;

        public NotificationUtility(ICourseService courseService, IAssessmentService assessmentService)
        {
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
            _assessmentService = assessmentService ?? throw new ArgumentNullException(nameof(assessmentService));
        }

        public async Task CheckAndNotifyAsync()
        {
            var today = DateTime.Today;

            var courses = await _courseService.GetEnabledNotificationsCoursesAsync();
            foreach (var course in courses)
            {
                if (course.StartDate.Date != today && course.StartDate.Date != today) { continue; }
                string message = $"Course '{course.Title}' is '{(course.StartDate.Date == today ? "starting" : "ending")}' today.";
                ScheduleNotification("Course Alert", message);
            }

            var assessments = await _assessmentService.GetEnabledNotificationsAssessmentsAsync();
            foreach (var assessment in assessments)
            {
                if (assessment.StartDate.Date != today && assessment.StartDate.Date != today) { continue; }
                string message = $"Assessment '{assessment.Name}' is '{(assessment.StartDate.Date == today ? "starting" : "ending")}' today.";
                ScheduleNotification("Assessment Alert", message);
            }
        }

        private async void ScheduleNotification(string title, string message)
        {
            var notification = new NotificationRequest
             {
                 Title = title,
                 Description = message,
             };
            await LocalNotificationCenter.Current.Show(notification);
        }
    }
}