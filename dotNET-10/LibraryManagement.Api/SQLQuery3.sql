USE LibraryApiDb
GO

CREATE TABLE Members (
    MemberId INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    MembershipDate DATETIME DEFAULT GETDATE()
);