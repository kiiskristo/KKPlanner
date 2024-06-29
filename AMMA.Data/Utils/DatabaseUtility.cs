using SQLite;
using AMMA.Data.Model;

namespace AMMA.Data.Utils;

    public class DatabaseUtility
    {
        private static DatabaseUtility? _instance;
        private SQLiteAsyncConnection? _database;

        public static DatabaseUtility Instance => _instance ??= new DatabaseUtility();

        private DatabaseUtility() { }

        public async Task InitializeDatabaseAsync(string path)
        {
            if (_database == null)
            {
                _database = new SQLiteAsyncConnection(path, Constants.Flags);
                await _database.CreateTableAsync<Term>();
                await _database.CreateTableAsync<Course>();
                await _database.CreateTableAsync<Assessment>();
                await _database.CreateTableAsync<Instructor>();
                await GenerateMockDataIfEmpty();
            }
        }
        
        public SQLiteAsyncConnection GetDatabaseConnection() => _database!;
        
        private async Task GenerateMockDataIfEmpty()
        {
            if (_database is null) { return; }
            var termCount = await _database.Table<Term>().CountAsync();
            if (termCount == 0)
            {
                var mockTerms = new List<Term>
                {
                    new Term { Title = "Term One", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(6) },
                    new Term { Title = "Term Two", StartDate = DateTime.Now.AddMonths(6), EndDate = DateTime.Now.AddMonths(12) },
                    new Term { Title = "Term Three", StartDate = DateTime.Now.AddMonths(12), EndDate = DateTime.Now.AddMonths(18) },
                    new Term { Title = "Term Four", StartDate = DateTime.Now.AddMonths(18), EndDate = DateTime.Now.AddMonths(24) },
                    new Term { Title = "Term Five", StartDate = DateTime.Now.AddMonths(24), EndDate = DateTime.Now.AddMonths(30) },
                    new Term { Title = "Term Six", StartDate = DateTime.Now.AddMonths(30), EndDate = DateTime.Now.AddMonths(36) },
                };
                foreach (var term in mockTerms)
                {
                    await _database.InsertAsync(term);
                }
            }
            
            var instructorsCount = await _database.Table<Instructor>().CountAsync();
            if (instructorsCount == 0)
            {
                var mockInstructors = new List<Instructor>
                {
                    new Instructor { Name = "John Doe", Email = "john.doe@example.com", PhoneNumber = "555-1234" },
                    new Instructor { Name = "Jane Smith", Email = "jane.smith@example.com", PhoneNumber = "555-5678" },
                    new Instructor { Name = "Michael Johnson", Email = "michael.johnson@example.com", PhoneNumber = "555-9012" },
                    new Instructor { Name = "Emily Davis", Email = "emily.davis@example.com", PhoneNumber = "555-3456" },
                    new Instructor { Name = "William Brown", Email = "william.brown@example.com", PhoneNumber = "555-7890" },
                    new Instructor { Name = "Linda Miller", Email = "linda.miller@example.com", PhoneNumber = "555-2345" },
                    new Instructor { Name = "David Wilson", Email = "david.wilson@example.com", PhoneNumber = "555-6789" },
                    new Instructor { Name = "Elizabeth Moore", Email = "elizabeth.moore@example.com", PhoneNumber = "555-0123" },
                };
                foreach (var instructor in mockInstructors)
                {
                    await _database.InsertAsync(instructor);
                }
            }
            
            var coursesCount = await _database.Table<Course>().CountAsync();
            if (coursesCount == 0)
            {
                var mockCourses = new List<Course>
                {
                    new Course { Title = "Software Security and Testing – D385", StartDate = DateTime.Parse("2024-07-29"), EndDate = DateTime.Parse("2025-07-25"), TermId = 3, InstructorId = 4, Status="Plan to Take", Notes="" },
                    new Course { Title = "Software I – C# – C968", StartDate = DateTime.Parse("2024-12-28"), EndDate = DateTime.Parse("2025-05-03"), TermId = 1, InstructorId = 8, Status="Completed", Notes="" },
                    new Course { Title = "Health, Fitness, and Wellness – C458", StartDate = DateTime.Parse("2024-04-19"), EndDate = DateTime.Parse("2026-03-25"), TermId = 2, InstructorId = 6, Status="In Progress", Notes="" },
                    new Course { Title = "User Experience Design – D479", StartDate = DateTime.Parse("2024-09-24"), EndDate = DateTime.Parse("2025-07-15"), TermId = 3, InstructorId = 7, Status="Completed", Notes="" },
                    new Course { Title = "Data Management - Foundations – D426", StartDate = DateTime.Parse("2024-04-16"), EndDate = DateTime.Parse("2025-07-07"), TermId = 1, InstructorId = 2, Status="Completed", Notes="" },
                    new Course { Title = "Data Structures and Algorithms I – C949", StartDate = DateTime.Parse("2024-10-01"), EndDate = DateTime.Parse("2026-03-24"), TermId = 4, InstructorId = 7, Status="In Progress", Notes="" },
                    new Course { Title = "Advanced Data Management – D326", StartDate = DateTime.Parse("2025-02-16"), EndDate = DateTime.Parse("2025-08-01"), TermId = 2, InstructorId = 8, Status="In Progress", Notes="" },
                    new Course { Title = "Network and Security - Foundations – D315", StartDate = DateTime.Parse("2024-05-29"), EndDate = DateTime.Parse("2025-09-18"), TermId = 1, InstructorId = 8, Status="Plan to Take", Notes="" },
                    new Course { Title = "American Politics and the US Constitution – C963", StartDate = DateTime.Parse("2024-10-27"), EndDate = DateTime.Parse("2025-07-31"), TermId = 3, InstructorId = 6, Status="Plan to Take", Notes="" },
                    new Course { Title = "Software Security and Testing – D385", StartDate = DateTime.Parse("2024-04-30"), EndDate = DateTime.Parse("2025-12-20"), TermId = 3, InstructorId = 2, Status="In Progress", Notes="" },
                    new Course { Title = "Business of IT - Applications – D336", StartDate = DateTime.Parse("2024-12-24"), EndDate = DateTime.Parse("2025-06-03"), TermId = 2, InstructorId = 7, Status="Plan to Take", Notes="" },
                    new Course { Title = "Web Development Foundations – D276", StartDate = DateTime.Parse("2024-11-02"), EndDate = DateTime.Parse("2025-08-22"), TermId = 2, InstructorId = 4, Status="Plan to Take", Notes="" }
                };

                foreach (var course in mockCourses)
                {
                    await _database.InsertAsync(course);
                }
            }
            
            var assessmentsCount = await _database.Table<Assessment>().CountAsync();
            if (assessmentsCount == 0)
            {
                var mockAssessments = new List<Assessment>
                {
                    new Assessment { Name = "First Assessment", StartDate = DateTime.Now.AddMonths(-8), EndDate = DateTime.Now.AddMonths(-8), Type = "Performance", EnableNotifications = true, Result = 82.5, CourseId = 1 },
                    new Assessment { Name = "Second Assessment", StartDate = DateTime.Now.AddMonths(-8), EndDate = DateTime.Now.AddMonths(-8), Type = "Objective", EnableNotifications = false, Result = 76.5, CourseId = 1 },
                    new Assessment { Name = "First Assessment", StartDate = DateTime.Now.AddMonths(-7), EndDate = DateTime.Now.AddMonths(-7), Type = "Performance", EnableNotifications = true, Result = 72.5, CourseId = 2 },
                    new Assessment { Name = "Second Assessment", StartDate = DateTime.Now.AddMonths(-7), EndDate = DateTime.Now.AddMonths(-7), Type = "Objective", EnableNotifications = false, Result = 70.5, CourseId = 2 },
                    new Assessment { Name = "First Assessment", StartDate = DateTime.Now.AddMonths(-6), EndDate = DateTime.Now.AddMonths(-6), Type = "Performance", EnableNotifications = true, Result = 66.5, CourseId = 3 },
                    new Assessment { Name = "Second Assessment", StartDate = DateTime.Now.AddMonths(-6), EndDate = DateTime.Now.AddMonths(-6), Type = "Objective", EnableNotifications = false, Result = 90.5, CourseId = 3 },
                    new Assessment { Name = "First Assessment", StartDate = DateTime.Now.AddMonths(-5), EndDate = DateTime.Now.AddMonths(-5), Type = "Performance", EnableNotifications = true, Result = 50.5, CourseId = 4 },
                    new Assessment { Name = "Second Assessment", StartDate = DateTime.Now.AddMonths(-5), EndDate = DateTime.Now.AddMonths(-5), Type = "Objective", EnableNotifications = false, Result = 70.5, CourseId = 4 },
                    new Assessment { Name = "First Assessment", StartDate = DateTime.Now.AddMonths(-4), EndDate = DateTime.Now.AddMonths(-4), Type = "Performance", EnableNotifications = true, Result = 81.5, CourseId = 5 },
                    new Assessment { Name = "Second Assessment", StartDate = DateTime.Now.AddMonths(-4), EndDate = DateTime.Now.AddMonths(-4), Type = "Objective", EnableNotifications = false, Result = 95.5, CourseId = 5 },
                    new Assessment { Name = "First Assessment", StartDate = DateTime.Now.AddMonths(-1), EndDate = DateTime.Now.AddMonths(-1), Type = "Performance", EnableNotifications = true, Result = 91.5, CourseId = 6 },
                    new Assessment { Name = "Second Assessment", StartDate = DateTime.Now.AddMonths(-1), EndDate = DateTime.Now.AddMonths(-1), Type = "Objective", EnableNotifications = false, Result = 78.5, CourseId = 6 },
                    new Assessment { Name = "First Assessment", StartDate = DateTime.Now.AddMonths(1), EndDate = DateTime.Now.AddMonths(1), Type = "Performance", EnableNotifications = true, Result = 85.5, CourseId = 7 },
                    new Assessment { Name = "Second Assessment", StartDate = DateTime.Now.AddMonths(1), EndDate = DateTime.Now.AddMonths(2), Type = "Objective", EnableNotifications = false, Result = 68.5, CourseId = 7 }
                    
                };
                foreach (var assessments in mockAssessments)
                {
                    await _database.InsertAsync(assessments);
                }
            }
        }
    }