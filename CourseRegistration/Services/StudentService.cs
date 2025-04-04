using CourseRegistration.Data;
using CourseRegistration.Models;

namespace CourseRegistration.Services
{

    public interface IStudentService
    {
        IEnumerable<Student> GetStudents();

        Student GetStudentById(int id);

        void SaveStudent(Student obj);

        void EditStudent(Student obj);

        void DeleteStudent(int id);

        bool StudentExist(int id);
    }






    public class StudentService : IStudentService
    {

        private readonly ApplicationDbContext _db;

        public StudentService(IConfiguration configuration, ApplicationDbContext db)
        {
            _db = db;
        }


        // METHODS

        public IEnumerable<Student> GetStudents()
        {
            return _db.Students;
        }

        public Student GetStudentById(int id)
        {
            return _db.Students.Find(id);
        }

        public void SaveStudent(Student obj)
        {
            _db.Students.Add(obj);
            _db.SaveChanges();
        }

        public void EditStudent(Student obj)
        {
            _db.Students.Update(obj);
            _db.SaveChanges();
        }

        public void DeleteStudent(int id)
        {
            Student obj = GetStudentById(id);

            _db.Students.Remove(obj);
            _db.SaveChanges();
        }


        public bool StudentExist(int id)
        {
            bool objExist = _db.Students.Any(e => e.Id == id);
            return objExist;
        }
    }
}
