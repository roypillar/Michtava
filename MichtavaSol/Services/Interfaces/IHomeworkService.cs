namespace Services.Interfaces
{
    using Entities.Models;
    using System;

    public interface IHomeworkService : IRepositoryService<Homework>
    {
        Homework GetById(Guid id);
    }
}
