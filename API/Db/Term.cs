using Microsoft.AspNetCore.Datasync.EFCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KKPlanner.API.Db;

public class Term : EntityTableData
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required, MinLength(1)]
    public string Title { get; set; }

    public DateTime StartDate { get; set; } = DateTime.Today;

    public DateTime EndDate { get; set; } = DateTime.Today;

    // Navigation properties
    public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
}