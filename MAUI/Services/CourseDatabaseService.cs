using MAUI.Model;
using MAUI.Utils;
using SQLite;

namespace MAUI.Services;

public interface ICourseService
{
    Task<List<Course>> GetEnabledNotificationsCoursesAsync();
    Task<List<Course>> GetAllCoursesAsync(bool noTermFilter = false);
    Task<Course?> GetCourseByIdAsync(int id);
    Task<int> RemoveTermIdById(int id);
    Task<int> SaveCourseAsync(Course course);
    Task<int> DeleteCourseAsync(Course course);
}

public class CourseDatabaseService: ICourseService
{
    private readonly SQLiteAsyncConnection _database = DatabaseUtility.Instance.GetDatabaseConnection();

    public async Task<List<Course>> GetEnabledNotificationsCoursesAsync()
    {
        return await _database.Table<Course>()
            .Where(c => c.EnableNotifications)
            .ToListAsync();
    }
    public async Task<List<Course>> GetAllCoursesAsync(bool noTermFilter = false)
    {
        if (noTermFilter)
        {
            return await _database.Table<Course>().Where(c => c.TermId == 0).ToListAsync();
        }
        else
        {
            return await _database.Table<Course>().ToListAsync();
        }
    }

    public async Task<Course?> GetCourseByIdAsync(int id)
    {
        var course =  await _database.Table<Course>().Where(i => i.Id == id).FirstOrDefaultAsync();
        if (course != null)
        {
            course.Assessments = await _database.Table<Assessment>().Where(x => x.CourseId == course.Id).ToListAsync();
        }
        return course;
    }
    
    public async Task<int> RemoveTermIdById(int id)
    {
        var course = await GetCourseByIdAsync(id);
        if (course is not null)
        {
            course.TermId = 0;
            return await SaveCourseAsync(course);
        }
        return 0;
    }

    public async Task<int> SaveCourseAsync(Course course)
    {
        if (course.Id != 0)
        {
            return await _database.UpdateAsync(course);
        }
        else
        {
            return await _database.InsertAsync(course);
        }
    }

    public async Task<int> DeleteCourseAsync(Course course)
    {
        return await _database.DeleteAsync(course);
    }
}
