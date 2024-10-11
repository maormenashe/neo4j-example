using Microsoft.AspNetCore.Mvc;

namespace Neo4jExample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseController(ILogger<CourseController> logger, CourseService courseService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCourses()
    {
        IEnumerable<Course> courses = await courseService.GetAllCoursesAsync();
        return Ok(courses);
    }

    [HttpGet("{identifier}")]
    public async Task<IActionResult> GetBookByTitle(string identifier)
    {
        Course? course = await courseService.GetCourseByIdentifierAsync(identifier);

        if (course is null)
            return NotFound();

        return Ok(course);
    }
}
