namespace Dal.Repositories
{
    using System;
    using System.Linq;
    using Entities.Models;
    using Interfaces;
    using System.Data.Entity;

    public class SchoolClassRepository : DeletableEntityRepository<SchoolClass>, ISchoolClassRepository
    {
        public SchoolClassRepository(IApplicationDbContext context) : base(context)
        {
        }


        //Assumes existence of student and schoolclass in system
        public bool doesStudentExistInClass(Student s, SchoolClass c)
        {
            SchoolClass attachedSC = this.All().Where(x => x.Id == c.Id).Include(y => y.Students).FirstOrDefault();
            foreach(Student stu in attachedSC.Students)
            {
                if (stu.Id == s.Id)
                    return true;
            }
            return false;
        }

        public SchoolClass GetByDetails(int classNumber, string letter)
        {
            if (classNumber <= 0 || classNumber > 100 || letter.Length != 1)
                return null;//robust this thing up

            SchoolClass schoolClass = this.All().
                Where(sc => sc.ClassNumber == classNumber &&
                         sc.ClassLetter == letter && sc.IsDeleted == false)
                .FirstOrDefault();
            return schoolClass;
        }
    }
}
