USE LibraryApiDb
GO 

CREATE TABLE Books (
    BookId INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(200),
    Author NVARCHAR(100),
    ISBN NVARCHAR(50),
    PublishedYear INT,
    Category NVARCHAR(50),
    CopiesAvailable INT
);