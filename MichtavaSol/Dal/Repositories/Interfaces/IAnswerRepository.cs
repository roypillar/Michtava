namespace Dal.Repositories.Interfaces
{
    using Entities.Models;
    using System;

    public interface IAnswerRepository : IDeletableEntityRepository<Answer>
    {
        Answer GetByHomeworkIdAndStudentId(Guid hwId, Guid studentId);
    }
}
