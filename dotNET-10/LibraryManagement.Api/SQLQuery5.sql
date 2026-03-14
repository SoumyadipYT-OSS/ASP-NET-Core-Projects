USE LibraryApiDb;
GO


CREATE PROCEDURE ReturnBook
    @TransactionId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE BorrowTransactions
    SET ReturnDate = GETDATE(),
        FineAmount = CASE 
                        WHEN GETDATE() > DueDate 
                        THEN DATEDIFF(DAY, DueDate, GETDATE()) * 10   -- example fine: 10 units per day late
                        ELSE 0 
                     END
    WHERE TransactionId = @TransactionId;
END
