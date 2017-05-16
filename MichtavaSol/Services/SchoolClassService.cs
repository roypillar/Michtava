namespace Services
{
    using System;
    using System.Linq;
    using Dal.Repositories.Interfaces;
    using Entities.Models;
    using Services.Interfaces;
    using Common;

    public class SchoolClassService : ISchoolClassService
    {
        private readonly ISchoolClassRepository schoolClassRepository;
        private readonly IStudentRepository studentRepository;

        public SchoolClassService(ISchoolClassRepository schoolClassRepository,
                                    IStudentRepository studentRepository)
        {
            this.schoolClassRepository = schoolClassRepository;
            this.studentRepository = studentRepository;

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

        public void Add(SchoolClass schoolClass)
        {
            this.schoolClassRepository.Add(schoolClass);
            this.schoolClassRepository.SaveChanges();
        }

        public void Update(SchoolClass schoolClass)
        {
            this.schoolClassRepository.Update(schoolClass);
            this.schoolClassRepository.SaveChanges();
        }

        public void Delete(SchoolClass schoolClass)
        {
            this.schoolClassRepository.Delete(schoolClass);
            this.schoolClassRepository.SaveChanges();
        }

        public void HardDelete(SchoolClass schoolClass)
        {
            this.schoolClassRepository.HardDelete(schoolClass);
            this.schoolClassRepository.SaveChanges();
        }

        public MichtavaResult addStudentToSchoolClass(Student s, SchoolClass c)//test
        {
            if (this.GetById(c.Id) == null)
                return new MichtavaFailure("Class was not found in system.");

            if (this.studentRepository.GetById(s.Id) == null)
                return new MichtavaFailure("Student was not found in system.");

            if (this.studentRepository.GetById(s.Id).SchoolClass != null)
                return new MichtavaFailure("Student is already in a class. \nRemove him from his class, then try again.");

            if (this.schoolClassRepository.doesStudentExistInClass(s, c))//TODO test these
                return new MichtavaFailure("Student already belongs in the given class");

            c.Students.Add(s);
            s.SchoolClass = c;

            this.schoolClassRepository.SaveChanges();
            this.studentRepository.SaveChanges();

            return new MichtavaSuccess("Student added to class successfully.");


        }
    }
}
