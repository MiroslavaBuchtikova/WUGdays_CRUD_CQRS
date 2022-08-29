using MediatR;
using University.Contracts.Dtos;

namespace University.Commands
{
    public class DisenrollCommand : IRequest<ResultDto>
    {
        public string SSN { get; }
        public int EnrollmentIndex { get; }
        public string Comment { get; }

        public DisenrollCommand(string ssn, int enrollmentIndex, string comment)
        {
            SSN = ssn;
            EnrollmentIndex = enrollmentIndex;
            Comment = comment;
        }
    }
}