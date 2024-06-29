using SQLite;

namespace MAUI.Model
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string? Title { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Today;

        public DateTime EndDate { get; set; } = DateTime.Today;

        [Ignore]
        public string DateRange => $"{StartDate:MMM dd, yyyy} - {EndDate:MMM dd, yyyy}";

        [Ignore]
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}