namespace Services.Interfaces
{
    using System;
    using Entities.Models;
    using Common;

    public interface ISchoolClassService : IRepositoryService<SchoolClass>
    {
        SchoolClass GetById(Guid id);

        SchoolClass GetByDetails(int gradeYear, string letter, int startYear);

         MichtavaResult addStudentToSchoolClass(Student s, SchoolClass c);
        
        void HardDelete(SchoolClass schoolClass);
    }
}
