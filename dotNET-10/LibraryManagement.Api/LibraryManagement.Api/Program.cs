using LibraryManagement.Api.Repositories;
using LibraryManagement.Api.Repositories.Interfaces;
using LibraryManagement.Api.Services;
using LibraryManagement.Api.Services.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Dependency Injection 
// repository
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();
builder.Services.AddScoped<IReportsRepository, ReportsRepository>();

// services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();
builder.Services.AddScoped<IReportsService, ReportsService>();

/*
// Database configuration 
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("LibraryApiDb")))
*/

// Configure IDbConnection for DI
builder.Services.AddScoped<IDbConnection>(sp => {
    var connectionString = builder.Configuration.GetConnectionString("LibraryApiDb");
    var conn = new SqlConnection(connectionString);
    conn.Open();   
    return conn;
});



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
