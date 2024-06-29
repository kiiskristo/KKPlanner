using SQLite;
using SQLiteNetExtensions.Attributes; 

namespace AMMA.Data.Model
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string? Title { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Today;

        public DateTime EndDate { get; set; } = DateTime.Today;
        [Ignore] 
        public bool IsSelected { get; set; } = false;

        [Ignore]
        public string DateRange => $"{StartDate:MMM dd, yyyy} - {EndDate:MMM dd, yyyy}";

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Course> Courses { get; set; } = [];
    }
}