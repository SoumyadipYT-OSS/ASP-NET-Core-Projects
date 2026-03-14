USE LibraryApiDb
GO


SELECT * FROM dbo.Books
GO

SELECT * FROM dbo.Members
GO 

SELECT * FROM dbo.BorrowTransactions
GO 



DECLARE @MemberId INT = 2;

SELECT 
    m.MemberId,
    m.Name,
    COUNT(bt.TransactionId) AS TotalBorrowed,
    SUM(CASE WHEN bt.ReturnDate IS NULL THEN 1 ELSE 0 END) AS CurrentlyBorrowed,
    SUM(CASE WHEN bt.ReturnDate IS NULL AND bt.DueDate < GETDATE() THEN 1 ELSE 0 END) AS OverdueCount
FROM Members m
LEFT JOIN BorrowTransactions bt ON m.MemberId = bt.MemberId
WHERE m.MemberId = @MemberId
GROUP BY m.MemberId, m.Name;
