using MAUI.Model;
using MAUI.Utils;
using SQLite;

namespace MAUI.Services;

public interface IAssessmentService
{
    Task<List<Assessment>> GetEnabledNotificationsAssessmentsAsync();
    Task<List<Assessment>> GetCourseAssessmentsAsync(int courseId);
    Task<Assessment> GetAssessmentByIdAsync(int id);
    Task<int> SaveAssessmentAsync(Assessment assessment);
    Task<int> DeleteAssessmentAsync(Assessment assessment);
}

public class AssessmentDatabaseService: IAssessmentService
{
    private readonly SQLiteAsyncConnection _database = DatabaseUtility.Instance.GetDatabaseConnection();

    public async Task<List<Assessment>> GetEnabledNotificationsAssessmentsAsync()
    {
        return await _database.Table<Assessment>()
            .Where(c => c.EnableNotifications)
            .ToListAsync();
    }
    public async Task<List<Assessment>> GetCourseAssessmentsAsync(int courseId)
    {
        return await _database.Table<Assessment>().Where(x => x.CourseId == courseId).ToListAsync();
    }

    public async Task<Assessment> GetAssessmentByIdAsync(int id)
    {
        return await _database.Table<Assessment>().Where(i => i.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveAssessmentAsync(Assessment assessment)
    {
        if (assessment.Id != 0)
        {
            return await _database.UpdateAsync(assessment);
        }
        else
        {
            return await _database.InsertAsync(assessment);
        }
    }

    public async Task<int> DeleteAssessmentAsync(Assessment assessment)
    {
        return await _database.DeleteAsync(assessment);
    }
}
