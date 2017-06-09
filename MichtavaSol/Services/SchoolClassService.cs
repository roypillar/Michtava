namespace Services
{
    using System;
    using System.Linq;
    using Dal.Repositories.Interfaces;
    using Entities.Models;
    using Services.Interfaces;
    using Common;
    using System.Data.Entity;
    using System.Collections.Generic;

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




            if (schoolClassRepository.Get(sc => sc.ClassNumber == schoolClass.ClassNumber &&
                                               sc.ClassLetter == schoolClass.ClassLetter).Count() > 1)
                return new MichtavaFailure();



            if (schoolClassRepository.Get(sc => sc.ClassNumber == schoolClass.ClassNumber &&
                                               sc.ClassLetter == schoolClass.ClassLetter).Count() == 1 &&
                                               schoolClassRepository.Get(sc => sc.ClassNumber == schoolClass.ClassNumber &&
                                           sc.ClassLetter == schoolClass.ClassLetter).FirstOrDefault().Id != schoolClass.Id)
                return new MichtavaFailure("לא ניתן לשנות את פרטי הכיתה לפרטים שכבר מצויים במערכת, אצל כיתה אחרת");



            this.schoolClassRepository.Update(schoolClass);
            this.schoolClassRepository.SaveChanges();
            return new MichtavaSuccess("כיתה עודכנה בהצלחה");



        }


        //null prevSc = just move the student to the target class
        //null targetSc = just remove the student from the class
        public MichtavaResult TransferStudentToClass(SchoolClass prevSc, Student student,SchoolClass targetSc)
        {

            var res1 = RemoveStudentFromClass(student, prevSc);

            if (res1 is MichtavaFailure)
                return res1;

            var res2 = AddStudentToClass(student, targetSc);

            if (res2 is MichtavaFailure)
                return res1;


            return new MichtavaSuccess("העברה בוצעה בהצלחה");



        }

     
        public MichtavaResult AddStudentToClass(Student student, SchoolClass targetSc)
        {
            if (student == null || targetSc == null)
                return new MichtavaFailure("recieved null argument/s");

            if (this.GetById(targetSc.Id) == null)
                return new MichtavaFailure("הכיתה לא נמצאה במערכת");

            if (this.studentRepository.GetById(student.Id) == null)
                return new MichtavaFailure("הסטודנט לא נמצא במערכת");



            SchoolClass psc = this.schoolClassRepository.All().Include(x => x.Students).FirstOrDefault(x => x.Id == targetSc.Id);
            Student pst = this.studentRepository.All().Include(x => x.SchoolClass).FirstOrDefault(x => x.Id == student.Id);

            if (psc.Students.ToList().Contains(pst))
                return new MichtavaSuccessWithWarning("הסטודנט כבר נמצא בכיתה.");

            if(pst.SchoolClass!=null)
                return new MichtavaFailure("חובה להסיר את הסטודנט מהכיתה שלו");



            psc.Students.Add(pst);
            pst.SchoolClass = psc;


            this.schoolClassRepository.Update(psc);

            this.schoolClassRepository.SaveChanges();


            this.studentRepository.Update(pst);
            this.studentRepository.SaveChanges();


            return new MichtavaSuccess("כיתה עודכנה בהצלחה");



        }

        public MichtavaResult RemoveStudentFromClass(Student student, SchoolClass sc)
        {
            if (student == null || sc == null)
                return new MichtavaFailure("recieved null argument/s");

            if (this.GetById(sc.Id) == null)
                return new MichtavaFailure("הכיתה לא נמצאה במערכת");

            if (this.studentRepository.GetById(student.Id) == null)
                return new MichtavaFailure("הסטודנט לא נמצא במערכת");



            SchoolClass psc = this.schoolClassRepository.All().Include(x => x.Students).FirstOrDefault(x => x.Id == sc.Id);
            Student pst = this.studentRepository.All().Include(x => x.SchoolClass).FirstOrDefault(x => x.Id == student.Id);

            if (!psc.Students.ToList().Contains(pst))
                return new MichtavaSuccessWithWarning("הכיתה לא מכילה את הסטודנט ");

            if (pst.SchoolClass == null)
                return new MichtavaFailure("הסטודנט לא נמצא בכיתה");



            psc.Students.Remove(pst);
            pst.SchoolClass = null;


            this.schoolClassRepository.Update(psc);

            this.schoolClassRepository.SaveChanges();


            this.studentRepository.Update(pst);
            this.studentRepository.SaveChanges();


            return new MichtavaSuccess("כיתה עודכנה בהצלחה");



        }



        public MichtavaResult AddTeacherToClass(Teacher teacher, SchoolClass targetSc)
        {
            if (teacher == null || targetSc == null)
                return new MichtavaFailure("recieved null argument/s");

            if (this.GetById(targetSc.Id) == null)
                return new MichtavaFailure("הכיתה לא נמצאה במערכת");

            if (this.teacherRepository.GetById(teacher.Id) == null)
                return new MichtavaFailure("המורה לא נמצא במערכת");



            SchoolClass psc = this.schoolClassRepository.All().Include(x => x.Teachers).FirstOrDefault(x => x.Id == targetSc.Id);
            Teacher pst = this.teacherRepository.All().Include(x => x.SchoolClasses).FirstOrDefault(x => x.Id == teacher.Id);

            if (psc.Teachers.ToList().Contains(pst))
                return new MichtavaSuccessWithWarning("המורה כבר נמצא בכיתה.");

            if (pst.SchoolClasses.ToList().Contains(psc))
                return new MichtavaFailure("המורה כבר נמצא בכיתה.");



            psc.Teachers.Add(pst);
            pst.SchoolClasses.Add(psc);


            this.schoolClassRepository.Update(psc);

            this.schoolClassRepository.SaveChanges();


            this.teacherRepository.Update(pst);
            this.teacherRepository.SaveChanges();


            return new MichtavaSuccess("כיתה עודכנה בהצלחה");



        }

        public MichtavaResult RemoveTeacherFromClass(Teacher teacher, SchoolClass targetSc)
        {
            if (teacher == null || targetSc == null)
                return new MichtavaFailure("recieved null argument/s");

            if (this.GetById(targetSc.Id) == null)
                return new MichtavaFailure("הכיתה לא נמצאה במערכת");

            if (this.teacherRepository.GetById(teacher.Id) == null)
                return new MichtavaFailure("המורה לא נמצא במערכת");



            SchoolClass psc = this.schoolClassRepository.All().Include(x => x.Teachers).FirstOrDefault(x => x.Id == targetSc.Id);
            Teacher pst = this.teacherRepository.All().Include(x => x.SchoolClasses).FirstOrDefault(x => x.Id == teacher.Id);

            if (!psc.Teachers.ToList().Contains(pst))
                return new MichtavaSuccessWithWarning("המורה לא נמצא בכיתה.");

            if (!pst.SchoolClasses.ToList().Contains(psc))
                return new MichtavaFailure("המורה לא נמצא בכיתה.");



            psc.Teachers.Remove(pst);
            pst.SchoolClasses.Remove(psc);


            this.schoolClassRepository.Update(psc);

            this.schoolClassRepository.SaveChanges();


            this.teacherRepository.Update(pst);
            this.teacherRepository.SaveChanges();


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
            var studentsIds = new List<Guid>();
            var teacherIds = new List<Guid>();

            foreach (Student s in students)
            {
                studentsIds.Add(s.Id);
            }

            foreach (Teacher t in teachers.ToList())
            {
                teacherIds.Add(t.Id);
            }

            foreach (Guid id in studentsIds)
            {
                Student stu = this.studentRepository.GetById(id);
                stu.SchoolClass = null;
                this.studentRepository.Update(stu);
            }

            foreach (Guid id in teacherIds)
            {
                Teacher t = this.teacherRepository.GetById(id);
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
            SchoolClass existing = this.schoolClassRepository.AllWithDeleted().Where(y => y.Id == schoolClass.Id).
                Include(x => x.Students).
                Include(x => x.Teachers).FirstOrDefault();


            if (existing == null)
                return new MichtavaFailure("הכיתה לא נמצאה במערכת");


            var students = existing.Students;
            var teachers = existing.Teachers;

            foreach (Student s in students.ToList())
            {
                s.SchoolClass = null;
                this.studentRepository.Update(s);

            }

            foreach (Teacher t in teachers.ToList())
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
