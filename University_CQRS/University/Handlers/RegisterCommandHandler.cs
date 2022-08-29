using MediatR;
using University.Commands;
using University.Contracts.Dtos;
using University.Persistence.Entities.Students;
using University.Persistence.Repositories;

namespace University.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ResultDto>
    {
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;
        private readonly StudentReadRepository _studentReadRepository;

        public RegisterCommandHandler(StudentRepository studentRepository,
            CourseRepository courseRepository, StudentReadRepository studentReadRepository)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _studentReadRepository = studentReadRepository;
        }

        public async Task<ResultDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (request.SSN == null)
            {
                throw new Exception($"SSN cant't be null");
            }

            var studentExists = _studentReadRepository.GetBySSN(request.SSN);
            if (studentExists != null)
            {
                throw new Exception("Student with this SSN already exists");
            }

            var student = new Student
            {
                Ssn = request.SSN,
                Name = request.Name,
                Email = request.Email
            };

            if (!string.IsNullOrEmpty(request.Course1))
            {
                AddEnrollment(request.Course1, request.Course1Grade, student);
            }

            if (!string.IsNullOrEmpty(request.Course2))
            {
                AddEnrollment(request.Course2, request.Course2Grade, student);
            }

            _studentRepository.Save(student);

            return new ResultDto(true);
        }

        public void AddEnrollment(string courseName, string grade, Student student)
        {
            Course course = _courseRepository.GetByName(courseName);
            if (student.Enrollments?.Count >= 2)
                throw new Exception("Cannot have more than 2 enrollments");

            var enrollment = new Enrollment
            {
                Course = course,
                Grade = Enum.Parse<Grade>(grade)
            };

            if (student.Enrollments == null)
            {
                student.Enrollments = new List<Enrollment>();
            }

            student.Enrollments.Add(enrollment);
        }
    }
}