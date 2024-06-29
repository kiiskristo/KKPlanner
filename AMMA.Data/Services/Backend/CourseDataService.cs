using System.Text;
using System.Text.Json;
using AMMA.Data.Model;
using AMMA.Data.Utils;

namespace AMMA.Data.Services.Backend;

public class CourseDataService : ICourseService
{
    private readonly HttpClient _httpClient = HttpClientUtility.Instance.GetHttpClient();

    public async Task<List<Course>> GetEnabledNotificationsCoursesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<List<Course>> GetAllCoursesAsync(bool noTermFilter = false)
    {
        var response = await _httpClient.GetAsync("tables/course");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Course>>(json) ?? [];
    }

    public async Task<Course?> GetCourseByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/tables/course/{id}");
        if (!response.IsSuccessStatusCode) { return null; }
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Course>(json);
    }

    public async Task<int> RemoveTermIdById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<int> SaveCourseAsync(Course course)
    {
        var json = JsonSerializer.Serialize(course);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        HttpResponseMessage response;
        if (course.Id == 0)
        {
            response = await _httpClient.PostAsync($"/tables/course", content);
        }
        else
        {
            response = await _httpClient.PutAsync($"/tables/course/{course.Id}", content);
        }
        
        if (!response.IsSuccessStatusCode) { return 0; }
        var savedTerm = JsonSerializer.Deserialize<Term>(await response.Content.ReadAsStringAsync());
        return savedTerm?.Id ?? 0;
    }

    public async Task<int> DeleteCourseAsync(Course course)
    {
        var response = await _httpClient.DeleteAsync($"/tables/course/{course.Id}");
        return response.IsSuccessStatusCode ? 1 : 0;
    }
}