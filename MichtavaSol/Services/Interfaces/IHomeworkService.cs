namespace Services.Interfaces
{
    using Common;
    using Entities.Models;
    using System;

    public interface IHomeworkService : IRepositoryService<Homework>
    {
        Homework GetById(Guid id);

        Homework GetByDetails(string title, string description);

        MichtavaResult HardDelete(Homework homework);
    }
}
