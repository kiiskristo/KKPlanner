using AMMA.Data.Model;

namespace AMMA.Data.Services.Backend;

public class AssessmentDataService : IAssessmentService
{
    public Task<List<Assessment>> GetEnabledNotificationsAssessmentsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<Assessment>> GetAllAssessmentsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<Assessment>> GetCourseAssessmentsAsync(int courseId)
    {
        throw new NotImplementedException();
    }

    public Task<Assessment> GetAssessmentByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveAssessmentAsync(Assessment assessment)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAssessmentAsync(Assessment assessment)
    {
        throw new NotImplementedException();
    }
}