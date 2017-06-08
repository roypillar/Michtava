using System.Threading.Tasks;
using System;
using Common;

namespace Services
{
    using System.Linq;
    using Dal.Repositories.Interfaces;
    using Entities.Models;
    using Services.Interfaces;

    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository teacherRepository;

        private readonly IApplicationUserRepository userRepository;

        public TeacherService(ITeacherRepository teacherRepository, IApplicationUserRepository userRepository)
        {
            this.teacherRepository = teacherRepository;
            this.userRepository = userRepository;
        }

        public IApplicationUserRepository UserRepository
        {
            get { return this.userRepository; }
        }

        public IQueryable<Teacher> All()
        {
            return this.teacherRepository.All();
        }


        public Teacher GetByUserName(string username)
        {
            return this.teacherRepository.All().FirstOrDefault(a => a.ApplicationUser.UserName == username && !a.IsDeleted);
        }

        public Teacher GetById(Guid id)
        {
            return this.teacherRepository.GetById(id);
        }

        public MichtavaResult Add(Teacher teacher)
        {
            if (teacher == null)
                return new MichtavaFailure("חייב לספק אובייקט ליצירה...");


            if (teacher.ApplicationUser == null && teacher.ApplicationUserId == null)
            {
                return new MichtavaFailure("must attach ApplicationUser before creation.");
            }

            //if (teacher.ApplicationUser.UserName == null || teacher.ApplicationUser.UserName == "")
            //    return new MichtavaFailure("חובה להזין שם משתמש.");


            //if (userRepository.Get(x => x.UserName == teacher.ApplicationUser.UserName).FirstOrDefault() == null)
            //    return new MichtavaFailure("please add ApplicationUser before using this function");

            this.teacherRepository.Add(teacher);
            this.teacherRepository.SaveChanges();
            return new MichtavaSuccess("משתמש נוסף בהצלחה");

        }

        public MichtavaResult Update(Teacher teacher)
        {
            if (teacher.ApplicationUser == null)
            {
                return new MichtavaFailure("must attach ApplicationUser.");
            }

            if (teacher.ApplicationUser.UserName == null || teacher.ApplicationUser.UserName == "")
                return new MichtavaFailure("חובה להזין שם משתמש.");

            if (userRepository.Get(sc => sc.UserName == teacher.ApplicationUser.UserName).Count() == 1 &&
                                              userRepository.Get(sc => sc.UserName == teacher.ApplicationUser.UserName).
                                              FirstOrDefault().Id != teacher.ApplicationUser.Id)
                return new MichtavaFailure("לא ניתן לשנות את פרטי המשתמש - שם המשתמש כבר קיים");




            this.teacherRepository.Update(teacher);
            this.teacherRepository.SaveChanges();
            return new MichtavaSuccess("משתמש עודכן בהצלחה");
        }


        public MichtavaResult Delete(Teacher teacher)
        {
            Teacher existing = this.teacherRepository.All().Where(y => y.Id == teacher.Id).
               FirstOrDefault();


            if (existing == null)
                return new MichtavaFailure("המשתמש לא נמצא במערכת");




            this.teacherRepository.Delete(teacher);
            this.teacherRepository.SaveChanges();
            //this.userRepository.Delete(teacher.ApplicationUser);

            //this.userRepository.SaveChanges();
            return new MichtavaSuccess("משתמש נמחק בהצלחה");

        }

        public MichtavaResult HardDelete(Teacher teacher)
        {

            Teacher existing = this.teacherRepository.AllWithDeleted().Where(y => y.Id == teacher.Id).
               FirstOrDefault();


            if (existing == null)
                return new MichtavaFailure("המשתמש לא נמצא במערכת");




            this.teacherRepository.HardDelete(teacher);
            this.teacherRepository.SaveChanges();
            //this.userRepository.HardDelete(teacher.ApplicationUser);

            //this.userRepository.SaveChanges();
            return new MichtavaSuccess("משתמש נמחק בהצלחה");
        }

        public IQueryable<Teacher> SearchByName(string searchString)
        {
            return this.teacherRepository.SearchByName(searchString);
        }

        public bool IsUserNameUniqueOnEdit(Teacher teacher, string username)
        {
            return this.teacherRepository.IsUserNameUniqueOnEdit(teacher, username);
        }

       


    }
}
