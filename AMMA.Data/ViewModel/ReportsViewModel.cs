using AMMA.Data.Services;
using AMMA.Data.Model;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AMMA.Data.ViewModel;

public partial class ReportsViewModel : ObservableObject
{
    private readonly ICourseService _courseService;
    private readonly IAssessmentService _assessmentService;

    [ObservableProperty]
    private ObservableCollection<GradeData> _averageGradeOverTime;

    [ObservableProperty]
    private ObservableCollection<StatusReport> _courseCompletionStatus;

    [ObservableProperty]
    private ObservableCollection<AssessmentTimeline> _upcomingAssessmentsTimeline;

    public ReportsViewModel(ICourseService courseService, IAssessmentService assessmentService)
    {
        _courseService = courseService;
        _assessmentService = assessmentService;
        _averageGradeOverTime = new ObservableCollection<GradeData>();
        _courseCompletionStatus = new ObservableCollection<StatusReport>();
        _upcomingAssessmentsTimeline = new ObservableCollection<AssessmentTimeline>();
    }

    public async void LoadReportsAsync()
    {
        var courses = await _courseService.GetAllCoursesAsync();
        var assessments = await _assessmentService.GetAllAssessmentsAsync();

        LoadAverageGradePerMonth(assessments);
        LoadCourseCompletionStatus(courses);
        LoadUpcomingAssessmentsTimeline(assessments);
    }

    private void LoadAverageGradePerMonth(IEnumerable<Assessment> assessments)
    {
        var monthlyAverageGrades = assessments
            .GroupBy(a => new { a.EndDate.Year, a.EndDate.Month })
            .Select(group => new GradeData
            {
                // Assuming you want to display the month in a "Year-Month" format.
                Date = new DateTime(group.Key.Year, group.Key.Month, 1),
                AverageGrade = group.Average(a => a.Result)
            })
            .OrderBy(g => g.Date) // Ensure the data is sorted by Date.
            .ToList();

        AverageGradeOverTime.Clear();
        foreach (var gradeData in monthlyAverageGrades)
        {
            AverageGradeOverTime.Add(gradeData);
        }
    }



    private void LoadCourseCompletionStatus(IEnumerable<Course> courses)
    {
        var statusReport = courses
            .GroupBy(course => course.Status)
            .Select(group => new StatusReport
            {
                Status = group.Key,
                Count = group.Count()
            });

        CourseCompletionStatus.Clear();
        foreach (var report in statusReport)
        {
            CourseCompletionStatus.Add(report);
        }
    }

    private void LoadUpcomingAssessmentsTimeline(IEnumerable<Assessment> assessments)
    {
        var upcomingAssessments = assessments
            .Where(assessment => assessment.EndDate > DateTime.Now)
            .Select(assessment => new AssessmentTimeline
            {
                AssessmentName = assessment.AssessmentName,
                DueDate = assessment.EndDate
            })
            .OrderBy(assessment => assessment.DueDate);

        UpcomingAssessmentsTimeline.Clear();
        foreach (var assessment in upcomingAssessments)
        {
            UpcomingAssessmentsTimeline.Add(assessment);
        }
    }
}

public class GradeData
{
    public DateTime Date { get; set; }
    public double AverageGrade { get; set; }
}

public class CourseReport
{
    public string? CourseTitle { get; set; }
    public double AverageGrade { get; set; }
}

public class StatusReport
{
    public string? Status { get; set; }
    public int Count { get; set; }
}

public class AssessmentTimeline
{
    public string? AssessmentName { get; set; }
    public DateTime DueDate { get; set; }
}
