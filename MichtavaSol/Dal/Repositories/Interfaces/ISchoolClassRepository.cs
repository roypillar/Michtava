namespace Dal.Repositories.Interfaces
{
    using Entities.Models;

    public interface ISchoolClassRepository : IDeletableEntityRepository<SchoolClass>
    {
        SchoolClass GetByDetails(int gradeYear, string letter);
        bool doesStudentExistInClass(Student s, SchoolClass c);

    }
}
