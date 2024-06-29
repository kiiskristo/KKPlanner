using MAUI.Model;
using MAUI.Utils;
using SQLite;

namespace MAUI.Services;

public interface ITermService
{
    Task<List<Term>> GetAllTermsAsync();
    Task<Term?> GetTermByIdAsync(int id);
    Task<int> SaveTermAsync(Term term);
    Task<int> DeleteTermAsync(Term term);
}

public class TermDatabaseService: ITermService
{
    private readonly SQLiteAsyncConnection _database = DatabaseUtility.Instance.GetDatabaseConnection();

    public async Task<List<Term>> GetAllTermsAsync()
    {
        return await _database.Table<Term>().ToListAsync();
    }

    public async Task<Term?> GetTermByIdAsync(int id)
    {
        var term = await _database.Table<Term>().Where(x => x.Id == id).FirstOrDefaultAsync();
        if (term != null)
        {
            term.Courses = await _database.Table<Course>().Where(x => x.TermId == term.Id).ToListAsync();
        }
        return term;
    }

    public async Task<int> SaveTermAsync(Term term)
    {
        if (term.Id != 0)
        {
            return await _database.UpdateAsync(term);
        }
        else
        {
            return await _database.InsertAsync(term);
        }
    }

    public async Task<int> DeleteTermAsync(Term term)
    {
        return await _database.DeleteAsync(term);
    }
}
