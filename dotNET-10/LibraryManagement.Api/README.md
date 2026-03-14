# Library Management System API

A complete **ASP.NET Core Web API** project for managing library operations such as borrowing, returning books, and generating reports.  
This project demonstrates clean architecture with Controllers → Services → Repositories → Database, using **Dapper** for data access and **SQL Server** as the backend.

---

## Features

### Borrowing & Returning
- **Borrow a book**  
  `POST /api/borrow`  
  Creates a new borrow transaction, decreases available copies, and validates availability.

- **Return a book**  
  `PUT /api/borrow/{transactionId}/return`  
  Updates the transaction with a return date, calculates fines via a stored procedure, and increments available copies.

- **View transactions**  
  - `GET /api/borrow` → List all borrow transactions.  
  - `GET /api/borrow/{transactionId}` → Get details of a specific transaction.

### Reports
- **Overdue books**  
  `GET /api/reports/overdue`  
  Lists all books that are overdue (`ReturnDate IS NULL AND DueDate < GETDATE()`).

- **Member borrowing stats**  
  `GET /api/reports/member-stats/{memberId}`  
  Shows total borrowed, currently borrowed, and overdue counts for a member.

---

## Architecture

- **Controllers**  
  Handle HTTP requests and responses (`BorrowController`, `ReportsController`).

- **Services**  
  Business logic (`BorrowService`, `ReportsService`).

- **Repositories**  
  Data access using Dapper (`BorrowRepository`, `BookRepository`, `ReportsRepository`).

- **Models**  
  Represent database entities (`Books`, `Members`, `BorrowTransactions`).

- **DTOs**  
  Data Transfer Objects for clean API responses (`BorrowRequestDto`, `BorrowResponseDto`, `MemberStatsDto`).

---

## Database Schema

### Tables
- **Books**
  - `BookId`, `Title`, `Author`, `Category`, `CopiesAvailable`
- **Members**
  - `MemberId`, `Name`, `Email`, `Phone`, `MembershipDate`
- **BorrowTransactions**
  - `TransactionId`, `BookId`, `MemberId`, `BorrowDate`, `DueDate`, `ReturnDate`, `FineAmount`

### Stored Procedure
```sql
CREATE PROCEDURE ReturnBook
    @TransactionId INT
AS
BEGIN
    UPDATE BorrowTransactions
    SET ReturnDate = GETDATE(),
        FineAmount = CASE 
                        WHEN GETDATE() > DueDate 
                        THEN DATEDIFF(DAY, DueDate, GETDATE()) * 10 
                        ELSE 0 
                     END
    WHERE TransactionId = @TransactionId;
END
```
---
---

## Tech Stack 
1. ASP.NET Core Web API ( .NET 10 )
2. Dapper (Micro ORM)
3. SQL Server
4. Dependency Injection
5. REST APIs

## Summary 
This project is a fully functional backend system for library management.
It demonstrates transaction-safe operations, clean architecture, and reporting features — making it an excellent portfolio project to showcase backend development skills with .NET, SQL, and Dapper.
