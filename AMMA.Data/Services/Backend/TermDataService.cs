using System.Text;
using System.Text.Json;
using AMMA.Data.Model;
using AMMA.Data.Utils;

namespace AMMA.Data.Services.Backend;

public class TermDataService: ITermService
{
    private readonly HttpClient _httpClient = HttpClientUtility.Instance.GetHttpClient();
    
    public async Task<List<Term>> GetAllTermsAsync()
    {
        var response = await _httpClient.GetAsync("tables/term");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Term>>(json) ?? [];
    }
    
    public async Task<Term?> GetTermByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/tables/term/{id}");
        if (!response.IsSuccessStatusCode) { return null; }
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Term>(json);
    }

    public async Task<int> SaveTermAsync(Term term)
    {
        var json = JsonSerializer.Serialize(term);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        HttpResponseMessage response;
        if (term.Id == 0)
        {
            response = await _httpClient.PostAsync($"/tables/term", content);
        }
        else
        {
            response = await _httpClient.PutAsync($"/tables/term/{term.Id}", content);
        }
        
        if (!response.IsSuccessStatusCode) { return 0; }
        var savedTerm = JsonSerializer.Deserialize<Term>(await response.Content.ReadAsStringAsync());
        return savedTerm?.Id ?? 0;
    }

    public async Task<int> DeleteTermAsync(Term term)
    {
        var response = await _httpClient.DeleteAsync($"/tables/term/{term.Id}");
        return response.IsSuccessStatusCode ? 1 : 0;
    }
}