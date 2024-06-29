using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Datasync.EFCore;

namespace KKPlanner.API.Db;

public class Assessment : EntityTableData
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string Name { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Type { get; set; }

    public bool EnableNotifications { get; set; } = false;

    // Foreign key and navigation property for Course
    public int CourseId { get; set; }

    [ForeignKey("CourseId")]
    public virtual Course Course { get; set; }
}