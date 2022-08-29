using Microsoft.EntityFrameworkCore;
using University.Contracts.Dtos;
using University.Persistence.Context;
using University.Persistence.Entities.Students;

namespace University.Persistence.Repositories
{
    public sealed class StudentReadRepository : GenericRepository<Student>
    {
        public StudentReadRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        public Student GetBySSN(string ssn)
        {
            return DbContext.Students
            .Include(x => x.Enrollments)
            .ThenInclude(x => x.Course)
            .Include(x => x.Disenrollments)
            .FirstOrDefault(w => w.Ssn == ssn);
        }

        public IReadOnlyList<StudentDto> GetList(string courseName, int? numberOfCourses)
        {
            var students = DbContext.Students.Include(x=>x.Enrollments)
                .ThenInclude(x=>x.Course)
                .Include(x=>x.Disenrollments)
            .ToList();
            var studentsDto = new List<StudentDto>();
            foreach (var student in students)
            {
                var studentDto = new StudentDto()
                {
                    Id = student.Id,
                    SSN = student.Ssn,
                    Name = student.Name,
                    Email = student.Email
                };
                if (student.Enrollments?.Count > 0)
                {
                    studentDto.Course1 = student.Enrollments?[0]?.Course?.Name;
                    studentDto.Course1Grade = student.Enrollments?[0]?.Grade.ToString();
                    studentDto.Course1Credits = student.Enrollments?[0]?.Course?.Credits;
                }
                if (student.Enrollments?.Count > 1)
                {

                    studentDto.Course2 = student.Enrollments?[1]?.Course?.Name;
                    studentDto.Course2Grade = student.Enrollments?[1]?.Grade.ToString();
                    studentDto.Course2Credits = student.Enrollments?[1]?.Course?.Credits;
                }

                studentsDto.Add(studentDto);
            }
           

            return studentsDto;
        }
    }
}
