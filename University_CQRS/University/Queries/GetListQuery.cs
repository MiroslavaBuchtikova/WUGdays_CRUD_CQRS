
using MediatR;
using University.Contracts.Dtos;

namespace University_CQRS.Queries
{
    public class GetListQuery : IRequest<List<StudentDto>>
    {
        public string CourseName { get; }

        public int? NumberOfCourses { get; }

        public GetListQuery(string courseName, int? numberOfCourses)
        {
            CourseName = courseName;
            NumberOfCourses = numberOfCourses;
        }
    }
}
