using Microsoft.AspNetCore.Mvc;
using University.Mapper;
using University.Persistence.Context;
using University.Persistence.Repositories;

namespace University.Controllers;

[ApiController]
[Route("[controller]")]
public class CourseController : ControllerBase
{
    private readonly CourseRepository _courseRepository;

    public CourseController(UniversityDbContext dbContext)
    {
        _courseRepository = new CourseRepository(dbContext);
    }

    [HttpGet]
    public IActionResult Get()
    {
        var courses = _courseRepository.GetList();
        var dtos = courses.Map();

        return Ok(dtos);
    }
}