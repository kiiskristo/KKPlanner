// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;

namespace KKPlanner.API.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureTerm(modelBuilder);
            ConfigureCourse(modelBuilder);
            ConfigureAssessment(modelBuilder);
            ConfigureInstructor(modelBuilder);
        }
        /// <summary>
        /// The dataset for the TodoItems.
        /// </summary>
        public DbSet<TodoItem> TodoItems => Set<TodoItem>();
        /// <summary>
        /// The dataset for the Terms.
        /// </summary>
        public DbSet<Term> Terms { get; set; }
        /// <summary>
        /// The dataset for the Courses.
        /// </summary>
        public DbSet<Course> Courses { get; set; }
        /// <summary>
        /// The dataset for the Assessments.
        /// </summary>
        public DbSet<Assessment> Assessments { get; set; }
        /// <summary>
        /// The dataset for the Instructors.
        /// </summary>
        public DbSet<Instructor> Instructors { get; set; }


        /// <summary>
        /// Do any database initialization required.
        /// </summary>
        /// <returns>A task that completes when the database is initialized</returns>
        public async Task InitializeDatabaseAsync()
        {
            await this.Database.EnsureCreatedAsync().ConfigureAwait(false);
        }
        
        private static void ConfigureTerm(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Term>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Term>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Term>()
                .Ignore(t => t.Deleted)
                .Ignore(t => t.UpdatedAt)
                .Ignore(t => t.Version);
        }
        
        private static void ConfigureCourse(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Course>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Course>()
                .Ignore(t => t.Deleted)
                .Ignore(t => t.UpdatedAt)
                .Ignore(t => t.Version);
        }
        
        private static void ConfigureAssessment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assessment>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Assessment>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Assessment>()
                .Ignore(t => t.Deleted)
                .Ignore(t => t.UpdatedAt)
                .Ignore(t => t.Version);
        }
        
        private static void ConfigureInstructor(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Instructor>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Instructor>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Instructor>()
                .Ignore(t => t.Deleted)
                .Ignore(t => t.UpdatedAt)
                .Ignore(t => t.Version);
        }
    }
}