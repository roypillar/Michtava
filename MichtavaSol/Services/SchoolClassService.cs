namespace Services
{
    using System;
    using System.Linq;
    using Dal.Repositories.Interfaces;
    using Entities.Models;
    using Services.Interfaces;
    using Common;
    using System.Data.Entity;

    public class SchoolClassService : ISchoolClassService
    {
        private readonly ISchoolClassRepository schoolClassRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ITeacherRepository teacherRepository;

        public SchoolClassService(ISchoolClassRepository schoolClassRepository,
                                    IStudentRepository studentRepository,
                                    ITeacherRepository teacherRepository
                                    )
        {
            this.schoolClassRepository = schoolClassRepository;
            this.studentRepository = studentRepository;
            this.teacherRepository = teacherRepository;

        }



        public IQueryable<SchoolClass> All()
        {
            return this.schoolClassRepository.All();
        }

        public SchoolClass GetById(Guid id)
        {
            return this.schoolClassRepository.GetById(id);
        }

        public SchoolClass GetByDetails(int gradeYear, string letter)
        {
            return this.schoolClassRepository.GetByDetails(gradeYear, letter);
        }

        public MichtavaResult Add(SchoolClass schoolClass)
        {
            if (schoolClassRepository.GetByDetails(schoolClass.ClassNumber, schoolClass.ClassLetter) != null)
                return new MichtavaFailure("כבר קיימת כיתה עם אותה האות ואותו המספר");

            this.schoolClassRepository.Add(schoolClass);
            this.schoolClassRepository.SaveChanges();
            return new MichtavaSuccess("כיתה נוספה בהצלחה");
        }

        public MichtavaResult Update(SchoolClass schoolClass)
        {
            if (this.GetById(schoolClass.Id) == null)
                return new MichtavaFailure("הכיתה לא נמצאה במערכת");

            //SchoolClass existing = GetById(schoolClass.Id);


            if(schoolClassRepository.Get(sc => sc.ClassNumber == schoolClass.ClassNumber &&
                                               sc.ClassLetter == schoolClass.ClassLetter).Count() ==0)
                return new MichtavaFailure();

            if (schoolClassRepository.Get(sc => sc.ClassNumber == schoolClass.ClassNumber &&
                                               sc.ClassLetter == schoolClass.ClassLetter).Count() > 1)
                return new MichtavaFailure();

            if (schoolClassRepository.Get(sc => sc.ClassNumber == schoolClass.ClassNumber &&
                                              sc.ClassLetter == schoolClass.ClassLetter).Count() == 1)
                return new MichtavaFailure("לא ניתן לשנות את פרטי הכיתה לפרטים שכבר מצויים במערכת, אצל כיתה אחרת");


            this.schoolClassRepository.Update(schoolClass);
            this.schoolClassRepository.SaveChanges();
            return new MichtavaSuccess("כיתה עודכנה בהצלחה");
        }


        //updates students and teachers - removes them from the class as well.
        public MichtavaResult Delete(SchoolClass schoolClass)
        {
            SchoolClass existing = this.schoolClassRepository.All().Where(y => y.Id == schoolClass.Id).
                Include(x => x.Students).
                Include(x => x.Teachers).FirstOrDefault();


            if (existing == null)
                return new MichtavaFailure("הכיתה לא נמצאה במערכת");


            var students = existing.Students;
            var teachers = existing.Teachers;

            foreach(Student s in students)
            {
                s.SchoolClass = null;
                this.studentRepository.Update(s);
                 
            }

            foreach(Teacher t in teachers)
            {
                t.SchoolClasses.Remove(existing);
                this.teacherRepository.Update(t);
            }



            
            this.schoolClassRepository.Delete(schoolClass);

            this.studentRepository.SaveChanges();
            this.teacherRepository.SaveChanges();
            this.schoolClassRepository.SaveChanges();



            return new MichtavaSuccess("כיתה נמחקה בהצלחה");
        }



        public MichtavaResult HardDelete(SchoolClass schoolClass)
        {
            SchoolClass existing = this.schoolClassRepository.All().Where(y => y.Id == schoolClass.Id).
                Include(x => x.Students).
                Include(x => x.Teachers).FirstOrDefault();


            if (existing == null)
                return new MichtavaFailure("הכיתה לא נמצאה במערכת");


            var students = existing.Students;
            var teachers = existing.Teachers;

            foreach (Student s in students)
            {
                s.SchoolClass = null;
                this.studentRepository.Update(s);

            }

            foreach (Teacher t in teachers)
            {
                t.SchoolClasses.Remove(existing);
                this.teacherRepository.Update(t);
            }




            this.schoolClassRepository.HardDelete(schoolClass);

            this.studentRepository.SaveChanges();
            this.teacherRepository.SaveChanges();
            this.schoolClassRepository.SaveChanges();



            return new MichtavaSuccess("כיתה נמחקה בהצלחה");
        }

        public MichtavaResult addStudentToSchoolClass(Student s, SchoolClass c)//test
        {
            if (this.GetById(c.Id) == null)
                return new MichtavaFailure("הכיתה לא נמצאה במערכת");

            if (this.studentRepository.GetById(s.Id) == null)
                return new MichtavaFailure("הסטודנט לא נמצא במערכת");

            if (this.studentRepository.GetById(s.Id).SchoolClass != null)
                return new MichtavaFailure("הסטודנט כבר שייך לכיתה/ יש להסיר אותו מהכיתה בה הוא נמצא ולנסות שנית. ");

            if (this.schoolClassRepository.doesStudentExistInClass(s, c))//TODO test these
                return new MichtavaFailure("הסטודנט כבר שייך לכיתה הזאת.");

            c.Students.Add(s);
            s.SchoolClass = c;

            this.schoolClassRepository.SaveChanges();
            this.studentRepository.SaveChanges();

            return new MichtavaSuccess("Student added to class successfully.");


        }
    }
}
