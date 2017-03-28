namespace Services.Interfaces
{
    using Entities.Models;
    public interface ISubjectService : IRepositoryService<Subject>
    {
        Subject GetById(int id);
    }
}
