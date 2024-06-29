using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Datasync.EFCore;

namespace KKPlanner.API.Db;

public class Instructor : EntityTableData
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string Name { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    // Assuming an instructor could be related to multiple courses
    public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
}