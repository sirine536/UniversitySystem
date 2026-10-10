INSERT INTO dbo.Users (Username, Password, RoleID, FullName,Department) 
VALUES ('admin', '123456', 1, 'Shaimaa Admin','computer science');

INSERT INTO dbo.Roles (RoleName) VALUES ('Admin'), ('Doctor'), ('Student');

INSERT INTO dbo.Programs (ProgramName) VALUES ('Computer Science'), ('IT');
INSERT INTO dbo.Courses (CourseName,CourseCode,Credits, ProgramID) VALUES ('Database Systems',123,3, 1), ('C# Programming',456,4, 1);

INSERT INTO dbo.Announcements (Title, MessageContent, PostDate) VALUES ('Welcome', 'Welcome to the Smart Portal', GETDATE());

