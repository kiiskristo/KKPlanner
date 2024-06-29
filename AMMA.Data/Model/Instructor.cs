using System.ComponentModel.DataAnnotations;
using SQLite;

namespace AMMA.Data.Model;

public class Instructor
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string? Name { get; set; } = "";
    public string? Email { get; set; } = "";
    public string? PhoneNumber { get; set; } = "";

}