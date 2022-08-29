using University.Contracts.Dtos;
using University.Persistence.Context;
using University.Persistence.Entities.Students;
using University.Persistence.Repositories;

namespace University.Services;

public class StudentService
{
    private readonly CourseRepository _courseRepository;

    public StudentService(UniversityDbContext dbContext)
    {
        _courseRepository = new CourseRepository(dbContext);
    }

    public void AppendEnrollments(Student student, StudentDto dto)
    {
        var firstEnrollment = GetEnrollment(student, 0);
        Append(dto.Course1, dto.Course1Grade, dto.Course1DisenrollmentComment, firstEnrollment, student);


        var secondEnrollment = GetEnrollment(student, 1);
        Append(dto.Course2, dto.Course2Grade, dto.Course2DisenrollmentComment, secondEnrollment, student);
    }

    private void Append(string newCourseName, string  newGrade, string courseDisenrollmentComment,
        Enrollment enrollment, Student student)
    {
        if (HasEnrollmentChanged(newCourseName, newGrade, enrollment))
        {
            if (string.IsNullOrWhiteSpace(newCourseName)) // Student disenrolls
            {
                if (string.IsNullOrWhiteSpace(courseDisenrollmentComment))
                    throw new Exception("Disenrollment comment is required");

                var enrollmentToRemove = enrollment;
                student.Enrollments.Remove(enrollmentToRemove);
                AddDisenrollmentWithComment(student, enrollment, courseDisenrollmentComment);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(newGrade))
                    throw new Exception("Grade is required");

                var course = _courseRepository.GetByName(newCourseName);

                if (enrollment == null)
                {
                    // Student enrolls
                    Enroll(student, course, Enum.Parse<Grade>(newGrade));
                }
                else
                {
                    // Student transfers
                    UpdateEnrollment(enrollment, course, Enum.Parse<Grade>(newGrade));
                }
            }
        }
    }

    private Enrollment GetEnrollment(Student student, int index)
    {
        return student.Enrollments?.Count > index ? student.Enrollments[index] : null;
    }

    private bool HasEnrollmentChanged(string newCourseName, string newGrade, Enrollment enrollment)
    {
        return enrollment != null
               && (newCourseName != enrollment.Course?.Name ||
                   newGrade != enrollment?.Grade.ToString());
    }

    private void AddDisenrollmentWithComment(Student student, Enrollment enrollment, string comment)
    {
        var disenrollment = new Disenrollment
        {
            Student = student,
            Course = enrollment.Course,
            Comment = comment,
            DateTime = DateTime.Now
        };
        student.Disenrollments ??= new List<Disenrollment>();

        student.Disenrollments.Add(disenrollment);
    }

    public void Enroll(Student student, Course course, Grade grade)
    {
        if (student.Enrollments?.Count >= 2)
            throw new Exception("Cannot have more than 2 enrollments");


        var enrollment = new Enrollment
        {
            Student = student,
            Course = course,
            Grade = grade
        };

        student.Enrollments ??= new List<Enrollment>();

        student.Enrollments.Add(enrollment);
    }

    private void UpdateEnrollment(Enrollment enrollment, Course course, Grade grade)
    {
        enrollment.Grade = grade;
        enrollment.Course = course;
    }
}