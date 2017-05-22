namespace Dal.Repositories.Interfaces
{
    using Entities.Models;

    public interface IHomeworkRepository : IDeletableEntityRepository<Homework>
    {
        Homework GetByDetails(string title, string description);
         
    }
}
