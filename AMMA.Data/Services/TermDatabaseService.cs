using AMMA.Data.Model;
using AMMA.Data.Utils;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

namespace AMMA.Data.Services;

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
        return await _database.GetAllWithChildrenAsync<Term>();
    }

    public async Task<Term?> GetTermByIdAsync(int id)
    {
        return await _database.GetWithChildrenAsync<Term>(id, recursive: true);
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
