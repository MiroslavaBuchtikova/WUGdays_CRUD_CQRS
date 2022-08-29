using MediatR;
using University.Commands;
using University.Contracts.Dtos;
using University.Persistence.Entities.Students;
using University.Persistence.Repositories;

namespace University.Handlers
{
    public class DisenrollCommandHandler : IRequestHandler<DisenrollCommand, ResultDto>
    {
        private readonly StudentRepository _studentRepository;
        private readonly StudentReadRepository _studentReadRepository;

        public DisenrollCommandHandler(StudentRepository studentRepository, StudentReadRepository studentReadRepository)
        {
            _studentRepository = studentRepository;
            _studentReadRepository = studentReadRepository;
        }

        public async Task<ResultDto> Handle(DisenrollCommand request, CancellationToken cancellationToken)
        {
            Student student = _studentReadRepository.GetBySSN(request.SSN);;
            if (student == null)
                throw new Exception($"No student found for SSN {request.SSN}");

            if (string.IsNullOrWhiteSpace(request.Comment))
                throw new Exception("Disenrollment comment is required");

            Enrollment enrollment = student.Enrollments.Count > request.EnrollmentIndex ? student.Enrollments[request.EnrollmentIndex] : null;
            if (enrollment == null)
                throw new Exception($"No enrollment found with number '{request.EnrollmentIndex}'");

            student.Enrollments.Remove(enrollment);

            var disenrollment = new Disenrollment
            {
                Student = student,
                Course = enrollment.Course,
                Comment = request.Comment,
                DateTime = DateTime.Now
            };
            if (student.Disenrollments == null)
            {
                student.Disenrollments = new List<Disenrollment>();
            }
            student.Disenrollments.Add(disenrollment);

            _studentRepository.Save(student);

            return new ResultDto(true);
        }
    }
}