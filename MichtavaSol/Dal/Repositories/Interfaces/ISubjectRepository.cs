namespace Dal.Repositories.Interfaces
{
    using Entities.Models;

    public interface ISubjectRepository : IDeletableEntityRepository<Subject>
    {
        Subject GetByName(string subjectName);
    }
}
