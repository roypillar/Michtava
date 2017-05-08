using System.Threading.Tasks;
using System;

namespace Services
{
    using System.Linq;
    using Dal.Repositories.Interfaces;
    using Entities.Models;
    using Services.Interfaces;

    public class StudentService : IStudentService
    {
        private readonly IStudentRepository studentRepository;

        private readonly IApplicationUserRepository userRepository;

        public StudentService(IStudentRepository studentRepository, IApplicationUserRepository userRepository)
        {
            this.studentRepository = studentRepository;
            this.userRepository = userRepository;
        }

        public IApplicationUserRepository UserRepository
        {
            get { return this.userRepository; }
        }

        public IQueryable<Student> All()
        {
            return this.studentRepository.All();
        }


        public Task<Student> GetByUserName(string username)
        {
            return this.studentRepository.GetByUserName(username);
        }

        public Student GetById(Guid id)
        {
            return this.studentRepository.GetById(id);
        }

        public void Add(Student student)
        {
            this.studentRepository.Add(student);
            this.studentRepository.SaveChanges();
        }

        public void Update(Student student)
        {
            this.studentRepository.Update(student);
            this.studentRepository.SaveChanges();
        }

        public void Delete(Student student)
        {
            student.ApplicationUser.DeletedBy = student.DeletedBy;

            this.userRepository.Delete(student.ApplicationUser);
            this.studentRepository.Delete(student);

            this.studentRepository.SaveChanges();
        }

        public IQueryable<Student> SearchByName(string searchString)
        {
            return this.studentRepository.SearchByName(searchString);
        }

        public bool IsUserNameUniqueOnEdit(Student student, string username)
        {
            return this.studentRepository.IsUserNameUniqueOnEdit(student, username);
        }

        public bool IsEmailUniqueOnEdit(Student student, string email)
        {
            return this.studentRepository.IsEmailUniqueOnEdit(student, email);
        }


    }
}
