namespace Services.Interfaces
{
    using System;
    using Entities.Models;

    public interface ISchoolClassService : IRepositoryService<SchoolClass>
    {
        SchoolClass GetById(Guid id);

        SchoolClass GetByDetails(int gradeYear, string letter, int startYear);

        void HardDelete(SchoolClass schoolClass);
    }
}
