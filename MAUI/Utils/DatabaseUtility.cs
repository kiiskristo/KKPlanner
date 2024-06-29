using SQLite;
using MAUI.Model;

namespace MAUI.Utils;

    public class DatabaseUtility
    {
        private static DatabaseUtility? _instance;
        private SQLiteAsyncConnection? _database;

        public static DatabaseUtility Instance => _instance ??= new DatabaseUtility();

        private DatabaseUtility() { }

        public async Task InitializeDatabaseAsync()
        {
            if (_database == null)
            {
                _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
                await _database.CreateTableAsync<Term>();
                await _database.CreateTableAsync<Course>();
                await _database.CreateTableAsync<Assessment>();
                await _database.CreateTableAsync<Instructor>();
                await GenerateMockDataIfEmpty();
            }
        }
        private async Task GenerateMockDataIfEmpty()
        {
            if (_database is null) { return; }
            var termCount = await _database.Table<Term>().CountAsync();
            if (termCount == 0)
            {
                var mockTerms = new List<Term>
                {
                    new Term { Title = "Term One", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(6) },
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
                    new Instructor { Name = "Anika Patel", Email = "anika.patel@strimeuniversity.edu", PhoneNumber = "555-123-4567" },
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
                    new Course { Title = "Mobile Application Development Using C#", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), TermId = 1, InstructorId = 1 }
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
                    new Assessment { Name = "First Assessment", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Type = "Performance", EnableNotifications = true, CourseId = 1},
                    new Assessment { Name = "Second Assessment", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Type = "Objective", EnableNotifications = true, CourseId = 1 }
                };
                foreach (var assessments in mockAssessments)
                {
                    await _database.InsertAsync(assessments);
                }
            }
        }
        
        public SQLiteAsyncConnection GetDatabaseConnection() => _database!;
    }