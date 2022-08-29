using MediatR;
using University.Commands;
using University.Contracts.Dtos;
using University.Persistence.Entities.Students;
using University.Persistence.Repositories;

namespace University.Handlers
{
    public class TransferCommandHandler : IRequestHandler<TransferCommand, ResultDto>
    {
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;
        private readonly StudentReadRepository _studentReadRepository;
        public TransferCommandHandler(StudentRepository studentRepository, CourseRepository courseRepository, StudentReadRepository studentReadRepository)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _studentReadRepository = studentReadRepository;
        }

        public async Task<ResultDto> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            Student student = _studentReadRepository.GetBySSN(request.SSN);;
            if (student == null)
                throw new Exception($"No student found with SSN '{request.SSN}'");

            Course course = _courseRepository.GetByName(request.Course);
            if (course == null)
                throw new Exception($"Course is incorrect: '{request.Course}'");

            bool success = Enum.TryParse(request.Grade, out Grade grade);
            if (!success)
                throw new Exception($"Grade is incorrect: '{request.Grade}'");

            Enrollment enrollment = student.Enrollments.Count > request.EnrollmentIndex ? student.Enrollments[request.EnrollmentIndex] : null;
            if (enrollment == null)
                throw new Exception($"No enrollment found with number '{request.EnrollmentIndex}'");

            enrollment.Course = course;
            enrollment.Grade = grade; 

             _studentRepository.Save(student);

            return new ResultDto(true);
        }
    }
}