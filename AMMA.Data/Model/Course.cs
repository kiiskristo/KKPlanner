using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace AMMA.Data.Model;
using SQLiteNetExtensions.Attributes; 

public class Course
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string? Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }

    [ForeignKey(typeof(Term))]
    public int? TermId { get; set; }
    
    [ForeignKey(typeof(Instructor))]
    public int? InstructorId { get; set; }
    
    public bool EnableNotifications { get; set; } = false;
    
    [OneToMany(CascadeOperations = CascadeOperation.All)]
    public List<Assessment> Assessments { get; set; } = [];
    [ManyToOne]
    public Term? Term { get; set; }
    [OneToOne]
    public Instructor? Instructor { get; set; }
}