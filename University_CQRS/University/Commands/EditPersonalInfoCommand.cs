using MediatR;
using University.Contracts.Dtos;

namespace University.Commands
{
    public sealed class EditPersonalInfoCommand : IRequest<ResultDto>
    {
        public string SSN { get; }
        public string Name { get; }
        public string Email { get; }

        public EditPersonalInfoCommand(string ssn, string name, string email)
        {
            SSN = ssn;
            Name = name;
            Email = email;

        }
    }
}
