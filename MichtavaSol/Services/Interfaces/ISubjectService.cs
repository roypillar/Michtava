namespace Services.Interfaces
{
    using Entities.Models;
    using System;

    public interface ISubjectService : IRepositoryService<Subject>
    {
        Subject GetById(Guid id);
        Subject GetByName(string subjectName);
    }
}
