-- 1. إضافة برامج دراسية (Programs)
INSERT INTO Programs (ProgramName) VALUES ('Computer Science');
INSERT INTO Programs (ProgramName) VALUES ('Business Administration');
INSERT INTO Programs (ProgramName) VALUES ('Graphic Design');

-- 2. إضافة كورسات وربطها بالبرامج
-- لنفترض CS أخد ID 1، والـ Business أخد ID 2
INSERT INTO Courses (CourseName, CourseCode, Credits, ProgramID) VALUES ('Data Structures', 'CS101', 3, 1);
INSERT INTO Courses (CourseName, CourseCode, Credits, ProgramID) VALUES ('Database Systems', 'CS202', 4, 1);
INSERT INTO Courses (CourseName, CourseCode, Credits, ProgramID) VALUES ('Marketing Basics', 'BUS101', 3, 2);
INSERT INTO Courses (CourseName, CourseCode, Credits, ProgramID) VALUES ('Accounting 1', 'BUS201', 3, 2);

-- 3. إضافة طلاب وربطهم ببرنامج معين
-- لنفترض RoleID للطلاب هو 3 (حسب مشروعك)
INSERT INTO Users (FullName, Email, Password, RoleID, ProgramID) 
VALUES ('Ahmad Mansour', 'ahmad@example.com', '123', 3, 1); -- طالب CS

INSERT INTO Users (FullName, Email, Password, RoleID, ProgramID) 
VALUES ('Sara Ibrahim', 'sara@example.com', '123', 3, 2); -- طالبة Business

-- 4. إضافة دكاترة (RoleID = 2)
INSERT INTO Users (FullName, Email, Password, RoleID) 
VALUES ('Dr. Samir', 'samir@example.com', '123', 2);