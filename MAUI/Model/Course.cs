using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace MAUI.Model;

public class Course
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string? Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }

    [ForeignKey(nameof(Term))]
    public int? TermId { get; set; }
    
    [ForeignKey(nameof(Instructor))]
    public int? InstructorId { get; set; }
    
    public bool EnableNotifications { get; set; } = false;
    
    [Ignore]
    public List<Assessment> Assessments { get; set; } = new List<Assessment>();
}