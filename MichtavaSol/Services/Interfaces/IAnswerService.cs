namespace Services.Interfaces
{
    using Entities.Models;
    using System;

    public interface IAnswerService : IRepositoryService<Answer>
    {
        Answer GetById(Guid id);
    }
}
