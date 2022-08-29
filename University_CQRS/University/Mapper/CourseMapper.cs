using University.Contracts.Dtos;
using University.Persistence.Entities.Students;

namespace University.Mapper;

public static class CourseMapper
{
    public static List<CourseDto> Map(this List<Course> courses)
    {
        return courses.Select(course => new CourseDto() 
            { Name = course.Name, Credits = course.Credits }).ToList();
    }
}