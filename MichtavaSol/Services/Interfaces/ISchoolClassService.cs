namespace Services.Interfaces
{
    using System;
    using Entities.Models;
    using Common;

    public interface ISchoolClassService : IRepositoryService<SchoolClass>
    {
        SchoolClass GetById(Guid id);

        SchoolClass GetByDetails(int gradeYear, string letter);

        MichtavaResult addStudentToSchoolClass(Student s, SchoolClass c);

        MichtavaResult HardDelete(SchoolClass schoolClass);
    }
}
