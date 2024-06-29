/*
* SQL database structure for Azure SQL
*/

CREATE TABLE Terms (
    Id INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(255),
    StartDate DATE,
    EndDate DATE
);

CREATE TABLE Courses (
    Id INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(255),
    StartDate DATE,
    EndDate DATE,
    Status NVARCHAR(255),
    Notes TEXT,
    TermId INT,
    InstructorId INT,
    EnableNotifications BIT,
    FOREIGN KEY (TermId) REFERENCES Terms(Id),
    FOREIGN KEY (InstructorId) REFERENCES Instructors(Id)
);

CREATE TABLE Assessments (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(255),
    StartDate DATE,
    EndDate DATE,
    Type NVARCHAR(50),
    EnableNotifications BIT,
    CourseId INT,
    FOREIGN KEY (CourseId) REFERENCES Courses(Id)
);

CREATE TABLE Instructors (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(255),
    Email NVARCHAR(255),
    PhoneNumber NVARCHAR(50)
);
