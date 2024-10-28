using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace StudentPortal.Models;

public class StudentCourseViewModel {
    public List<Student>? Students { get; set; }
    public SelectList? Courses { get; set; }
    public string? Course { get; set; }
    public string? SearchString { get; set; }
}
