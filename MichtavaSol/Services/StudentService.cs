using System.Threading.Tasks;
using System;
using Common;

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


        public Student GetByUserName(string username)
        {
            return this.studentRepository.All().FirstOrDefault(a => a.ApplicationUser.UserName == username && !a.IsDeleted);
        }

        public Student GetById(Guid id)
        {
            return this.studentRepository.GetById(id);
        }

        public MichtavaResult Add(Student student)
        {
            if (student == null)
                return new MichtavaFailure("חייב לספק אובייקט ליצירה...");


            if (student.ApplicationUser == null)
            {
                return new MichtavaFailure("must attach ApplicationUser before creation.");
            }

            if (student.ApplicationUser.UserName == null || student.ApplicationUser.UserName == "")
                return new MichtavaFailure("חובה להזין שם משתמש.");


            if (userRepository.Get(x => x.UserName == student.ApplicationUser.UserName).FirstOrDefault() == null)
                return new MichtavaFailure("please add ApplicationUser before using this function");

            this.studentRepository.Add(student);
            this.studentRepository.SaveChanges();
            return new MichtavaSuccess("משתמש נוסף בהצלחה");

        }

        public MichtavaResult Update(Student student)
        {
            if (student.ApplicationUser == null)
            {
                return new MichtavaFailure("must attach ApplicationUser.");
            }

            if (student.ApplicationUser.UserName == null || student.ApplicationUser.UserName == "")
                return new MichtavaFailure("חובה להזין שם משתמש.");

            if (userRepository.Get(sc => sc.UserName == student.ApplicationUser.UserName).Count() == 1 &&
                                              userRepository.Get(sc => sc.UserName == student.ApplicationUser.UserName).
                                              FirstOrDefault().Id != student.ApplicationUser.Id)
                return new MichtavaFailure("לא ניתן לשנות את פרטי המשתמש - שם המשתמש כבר קיים");




            this.studentRepository.Update(student);
            this.studentRepository.SaveChanges();
            return new MichtavaSuccess("משתמש עודכן בהצלחה");
        }


        public MichtavaResult Delete(Student student)
        {
            Student existing = this.studentRepository.All().Where(y => y.Id == student.Id).
               FirstOrDefault();


            if (existing == null)
                return new MichtavaFailure("המשתמש לא נמצא במערכת");




            this.studentRepository.Delete(student);
            this.studentRepository.SaveChanges();
            //this.userRepository.Delete(student.ApplicationUser);

            //this.userRepository.SaveChanges();
            return new MichtavaSuccess("משתמש נמחק בהצלחה");

        }

        public MichtavaResult HardDelete(Student student)
        {

            Student existing = this.studentRepository.AllWithDeleted().Where(y => y.Id == student.Id).
               FirstOrDefault();


            if (existing == null)
                return new MichtavaFailure("המשתמש לא נמצא במערכת");




            this.studentRepository.HardDelete(student);
            this.studentRepository.SaveChanges();
            //this.userRepository.HardDelete(student.ApplicationUser);

            //this.userRepository.SaveChanges();
            return new MichtavaSuccess("משתמש נמחק בהצלחה");
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
