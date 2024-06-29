using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Datasync.EFCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace KKPlanner.API.Db;

public class Course : EntityTableData
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string Title { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; }

    public string Notes { get; set; }

    public bool EnableNotifications { get; set; } = false;

    // Foreign key and navigation property for Term
    public int TermId { get; set; }

    [ForeignKey("TermId")]
    public virtual Term Term { get; set; }

    // Foreign key and navigation property for Instructor
    public int InstructorId { get; set; }

    [ForeignKey("InstructorId")]
    public virtual Instructor Instructor { get; set; }

    // Navigation property for assessments
    public virtual ICollection<Assessment> Assessments { get; set; } = new HashSet<Assessment>();
}