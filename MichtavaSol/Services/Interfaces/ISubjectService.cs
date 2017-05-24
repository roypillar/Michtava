namespace Services.Interfaces
{
    using Common;
    using Entities.Models;
    using System;

    public interface ISubjectService : IRepositoryService<Subject>
    {
        Subject GetById(Guid id);
        Subject GetByName(string subjectName);

        MichtavaResult HardDelete(Subject subject);
    }
}
