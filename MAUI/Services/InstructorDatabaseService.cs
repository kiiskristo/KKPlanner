using MAUI.Model;
using MAUI.Utils;
using SQLite;

namespace MAUI.Services;

public interface IInstructorService
{
    Task<List<Instructor>> GetAllInstructorsAsync();
    Task<Instructor> GetInstructorByIdAsync(int id);
    Task<int> SaveInstructorAsync(Instructor instructor);
    Task<int> DeleteInstructorAsync(Instructor instructor);
}

public class InstructorDatabaseService: IInstructorService
{
    private readonly SQLiteAsyncConnection _database = DatabaseUtility.Instance.GetDatabaseConnection();
    
    public async Task<List<Instructor>> GetAllInstructorsAsync()
    {
        return await _database.Table<Instructor>().ToListAsync();
    }

    public async Task<Instructor> GetInstructorByIdAsync(int id)
    {
        return await _database.Table<Instructor>().Where(i => i.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveInstructorAsync(Instructor instructor)
    {
        if (instructor.Id != 0)
        {
            return await _database.UpdateAsync(instructor);
        }
        else
        {
            return await _database.InsertAsync(instructor);
        }
    }

    public async Task<int> DeleteInstructorAsync(Instructor instructor)
    {
        return await _database.DeleteAsync(instructor);
    }
}
