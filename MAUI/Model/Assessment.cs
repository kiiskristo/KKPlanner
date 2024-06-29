using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MAUI.Model;

public class Assessment
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string? Name { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Type { get; set; } = "";
    public bool EnableNotifications { get; set; } = false;

    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }
    [Ignore]
    public string AssessmentName => $"{(Type == "Performance" ? "PA" : "OA")} - {Name}";

}