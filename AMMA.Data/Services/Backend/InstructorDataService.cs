using AMMA.Data.Model;
using MAUI.Services;

namespace AMMA.Data.Services.Backend;

public class InstructorDataService : IInstructorService
{
    public Task<List<Instructor>> GetAllInstructorsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Instructor> GetInstructorByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveInstructorAsync(Instructor instructor)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteInstructorAsync(Instructor instructor)
    {
        throw new NotImplementedException();
    }
}