namespace Services.Interfaces
{
    using Entities.Models;
    using System;

    public interface ITextService : IRepositoryService<Text>
    {
        Text GetById(Guid id);

    }
}
