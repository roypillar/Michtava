namespace Services.Interfaces
{
    using Entities.Models;
    public interface IAnswerService : IRepositoryService<Answer>
    {
        Answer GetById(int id);
    }
}
