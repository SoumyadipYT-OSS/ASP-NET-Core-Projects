using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Models;

namespace StudentPortal.Data
{
    public class StudentManagementContext : DbContext
    {
        public StudentManagementContext (DbContextOptions<StudentManagementContext> options)
            : base(options)
        {
        }

        public DbSet<StudentPortal.Models.Student> Student { get; set; } = default!;
    }
}
