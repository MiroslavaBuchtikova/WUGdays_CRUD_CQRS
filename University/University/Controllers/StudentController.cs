﻿using Microsoft.AspNetCore.Mvc;
using University.Contracts.Dtos;
using University.Mapper;
using University.Persistence.Context;
using University.Persistence.Entities.Students;
using University.Persistence.Repositories;
using University.Services;

namespace University.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private readonly StudentRepository _studentRepository;
    private readonly CourseRepository _courseRepository;
    private readonly StudentService _studentService;

    public StudentController(UniversityDbContext dbContext, StudentService studentService)
    {
        _studentRepository = new StudentRepository(dbContext);
        _courseRepository = new CourseRepository(dbContext);
        _studentService = studentService;
    }

    [HttpGet]
    public IActionResult Get(string courseName)
    {
        var students = _studentRepository.GetList(courseName);
        var dtos = students.Select(x => x.Map()).ToList();

        return Ok(dtos);
    }

    [HttpPost]
    public IActionResult Create([FromBody] StudentDto dto)
    {
        if (dto.Ssn == null)
        {
            return BadRequest($"SSN can't be null");
        }

        var existingStudent = _studentRepository.GetBySSN(dto.Ssn);
        if (existingStudent != null)
        {
            return BadRequest($"Student with SSN {dto.Ssn} already exists");
        }

        var student = new Student
        {
            Ssn = dto.Ssn,
            Name = dto.Name,
            Email = dto.Email
        };

        if (dto.Course1 != null && dto.Course1Grade != null)
        {
            var course = _courseRepository.GetByName(dto.Course1);
            _studentService.Enroll(student, course, Enum.Parse<Grade>(dto.Course1Grade));
        }

        if (dto.Course2 != null && dto.Course2Grade != null)
        {
            var course = _courseRepository.GetByName(dto.Course2);
            _studentService.Enroll(student, course, Enum.Parse<Grade>(dto.Course2Grade));
        }

        _studentRepository.Save(student);

        return Ok(student.Map());
    }

    [HttpDelete("{ssn}")]
    public IActionResult Delete(string ssn)
    {
        var student = _studentRepository.GetBySSN(ssn);
        if (student == null)
            return BadRequest($"No student found for SSN {ssn}");

        _studentRepository.Delete(student);

        return Ok();
    }

    [HttpPut("{ssn}")]
    public IActionResult Update(string ssn, [FromBody] StudentDto dto)
    {
        var student = _studentRepository.GetBySSN(ssn);
        if (student == null)
            return BadRequest($"No student found for SSN {ssn}");

        student.Name = dto.Name;
        student.Email = dto.Email;

        _studentService.AppendEnrollments(student, dto);
        _studentRepository.Save(student);

        return Ok(student.Map());
    }
}