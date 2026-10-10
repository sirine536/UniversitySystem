-- 1. Create the table to store messages
CREATE TABLE Announcements (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200),
    MessageContent NVARCHAR(MAX),
    PostDate DATETIME DEFAULT GETDATE(),
    AdminID INT -- Optional: to know which admin sent it
);

-- 2. Optional: Insert a test message
INSERT INTO Announcements (Title, MessageContent) 
VALUES ('Welcome', 'This is a test notification for all students.');