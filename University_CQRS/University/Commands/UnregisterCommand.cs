using MediatR;
using University.Contracts.Dtos;

namespace University.Commands
{
    public sealed class UnregisterCommand : IRequest<ResultDto>
    {
        public string SSN { get; }

        public UnregisterCommand(string ssn)
        {
            SSN = ssn;
        }
    }
}