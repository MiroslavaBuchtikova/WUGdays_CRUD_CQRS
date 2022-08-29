using MediatR;
using University.Commands;
using University.Contracts.Dtos;
using University.Persistence.Entities.Students;
using University.Persistence.Repositories;

namespace University.Handlers
{
    public class UnregisterCommandHandler : IRequestHandler<UnregisterCommand, ResultDto>
    {
        private readonly StudentRepository _studentRepository;
        private readonly StudentReadRepository _studentReadRepository;

        public UnregisterCommandHandler(StudentRepository studentRepository, StudentReadRepository studentReadRepository)
        {
            _studentRepository = studentRepository;
            _studentReadRepository = studentReadRepository;
        }

        public Task<ResultDto> Handle(UnregisterCommand request, CancellationToken cancellationToken)
        {
            Student student = _studentReadRepository.GetBySSN(request.SSN);;
            if (student == null)
                throw new Exception($"No student found for SSN {request.SSN}");

            _studentRepository.Delete(student);

            return Task.FromResult(new ResultDto(true));
        }
    }
}