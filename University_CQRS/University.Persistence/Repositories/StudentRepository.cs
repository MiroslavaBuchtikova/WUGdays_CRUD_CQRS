using University.Persistence.Context;
using University.Persistence.Entities.Students;

namespace University.Persistence.Repositories
{
    public sealed class StudentRepository : GenericRepository<Student>
    {
        public StudentRepository(UniversityDbContext dbContext) : base(dbContext)
        {
        }

        public void Save(Student student)
        {
            DbContext.Update(student);
            DbContext.SaveChanges();
        }

        public void Delete(Student student)
        {
            DbContext.Remove(student);
            DbContext.SaveChanges();
        }
    }
}
