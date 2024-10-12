using Microsoft.AspNetCore.Mvc;

namespace Neo4jExample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseController(ILogger<CourseController> logger, CourseService courseService, LessonService lessonService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCourses()
    {
        IEnumerable<Course> courses = await courseService.GetAllCoursesAsync();
        return Ok(courses);
    }

    [HttpGet("{identifier}")]
    public async Task<IActionResult> GetCourseByIdentifier(string identifier)
    {
        Course? course = await courseService.GetCourseByIdentifierAsync(identifier);

        if (course is null)
            return NotFound();

        IEnumerable<Lesson> lessons = await lessonService.GetLessonsByCourseIdentifierAsync(identifier);

        course.lessons = lessons.ToList();

        return Ok(course);
    }

    // POST: api/course/{courseIdentifier}/lessons/{lessonIdentifier}
    [HttpPost("{courseIdentifier}/lessons/{lessonIdentifier}")]
    public async Task<IActionResult> AddLessonToCourse(string courseIdentifier, string lessonIdentifier, [FromBody] BelongsTo belongsTo)
    {
        try
        {
            await courseService.CreateBelongsToRelationAsync(courseIdentifier, lessonIdentifier, belongsTo);
            return Ok("Lesson added to course successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }

    [HttpGet("/lessons")]
    public async Task<IActionResult> GetAllCoursesWithLessons()
    {
        try
        {
            var coursesWithLessons = await courseService.GetAllCoursesWithLessonsAsync();
            return Ok(coursesWithLessons);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }

    [HttpGet("{courseIdentifier}/lessons")]
    public async Task<IActionResult> GetCourseWithLessons(string courseIdentifier)
    {
        try
        {
            var coursesWithLessons = await courseService.GetCourseWithLessonsAsync(courseIdentifier);
            if (coursesWithLessons == null)
                return NotFound();

            return Ok(coursesWithLessons);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }
}
