using MediatR;
using University_CQRS.Queries;
using University.Contracts.Dtos;
using University.Persistence.Repositories;

namespace University.Handlers
{
    public class GetListQueryHandler : IRequestHandler<GetListQuery, List<StudentDto>>
    {
        private readonly StudentReadRepository _studentReadRepository;

        public GetListQueryHandler(StudentReadRepository studentReadRepository)
        {
            _studentReadRepository = studentReadRepository;
        }

        public Task<List<StudentDto>> Handle(GetListQuery request, CancellationToken cancellationToken)
        {
            var students = _studentReadRepository.GetList(request.CourseName, request.NumberOfCourses).ToList();

            return Task.FromResult(students);
        }
    }
}