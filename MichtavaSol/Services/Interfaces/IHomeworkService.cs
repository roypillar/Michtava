namespace Services.Interfaces
{
    using Entities.Models;

    public interface IHomeworkService : IRepositoryService<Homework>
    {
        Homework GetById(int id);
    }
}
