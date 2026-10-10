-- 1. Create Programs Table
CREATE TABLE Programs (
    ProgramID INT PRIMARY KEY IDENTITY(1,1),
    ProgramName NVARCHAR(100) NOT NULL -- Example: 'Computer Science'
);

-- 2. Link Courses to Programs
ALTER TABLE Courses ADD ProgramID INT;

-- 3. Link Student to a Program
ALTER TABLE Users ADD ProgramID INT;