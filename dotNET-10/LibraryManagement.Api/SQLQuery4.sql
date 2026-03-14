USE LibraryApiDb
GO

CREATE TABLE BorrowTransactions (
    TransactionId INT PRIMARY KEY IDENTITY,
    BookId INT FOREIGN KEY REFERENCES Books(BookId),
    MemberId INT FOREIGN KEY REFERENCES Members(MemberId),
    BorrowDate DATETIME DEFAULT GETDATE(),
    DueDate DATETIME,
    ReturnDate DATETIME NULL,
    FineAmount DECIMAL(10,2) DEFAULT 0
);