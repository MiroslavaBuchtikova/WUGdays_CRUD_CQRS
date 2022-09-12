using MediatR;
using Microsoft.AspNetCore.Mvc;
using University_CQRS.Queries;
using University.Commands;
using University.Contracts.Dtos;

namespace University.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public IActionResult GetList(string courseName, int? numberOfCourses)
    {
        var result = _mediator.Send(new GetListQuery(courseName, numberOfCourses));

        return Ok(result.Result);
    }

    [HttpPost]
    public IActionResult Register([FromBody] NewStudentDto dto)
    {
        var result = _mediator.Send(new RegisterCommand(
            dto.SSN,
            dto.Name, dto.Email,
            dto.Course1, dto.Course1Grade,
            dto.Course2, dto.Course2Grade));

        return Ok(result.Result);
    }

    [HttpDelete("{ssn}")]
    public IActionResult Unregister(string ssn)
    {
        var result = _mediator.Send(new UnregisterCommand(ssn));
        return Ok(result.Result);
    }

    [HttpPost("{ssn}/enrollments")]
    public IActionResult Enroll(string ssn, [FromBody] StudentEnrollmentDto dto)
    {
        var result = _mediator.Send(new EnrollCommand(ssn, dto.Course, dto.Grade));
        return Ok(result.Result);
    }

    [HttpPut("{ssn}/enrollments/{enrollmentIndex}")]
    public IActionResult Transfer(
        string ssn, int enrollmentIndex, [FromBody] StudentTransferDto dto)
    {
        var result = _mediator.Send(new TransferCommand(ssn, enrollmentIndex, dto.Course, dto.Grade));
        return Ok(result.Result);
    }

    [HttpPost("{ssn}/enrollments/{enrollmentIndex}/disenroll")]
    public IActionResult Disenroll(
        string ssn, int enrollmentNumber, [FromBody] StudentDisenrollmentDto dto)
    {
        var result = _mediator.Send(new DisenrollCommand(ssn, enrollmentNumber, dto.Comment));
        return Ok(result.Result);
    }

    [HttpPut("{ssn}")]
    public IActionResult EditPersonalInfo(string ssn, [FromBody] StudentPersonalInfoDto dto)
    {
        var result = _mediator.Send(new EditPersonalInfoCommand(ssn, dto.Name, dto.Email));
        return Ok(result.Result);
    }
}