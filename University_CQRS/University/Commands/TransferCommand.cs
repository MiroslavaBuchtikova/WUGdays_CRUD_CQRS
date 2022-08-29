using MediatR;
using University.Contracts.Dtos;

namespace University.Commands
{
    public sealed class TransferCommand : IRequest<ResultDto>
    {
        public string SSN { get; }
        public int EnrollmentIndex { get; }
        public string Course { get; }
        public string Grade { get; }

        public TransferCommand(string ssn, int enrollmentIndex, string course, string grade)
        {
            SSN = ssn;
            EnrollmentIndex = enrollmentIndex;
            Course = course;
            Grade = grade;
        }
    }
}