namespace Dal.Repositories
{
    using Entities.Models;
    using Dal.Repositories.Interfaces;
    using System;
    using System.Linq;

    public class AnswerRepository : DeletableEntityRepository<Answer>, IAnswerRepository
    {
        public AnswerRepository(IApplicationDbContext context) : base(context)
        {
        }

        public Answer GetByHomeworkIdAndStudentId(Guid hwId, Guid studentId)
        {
            return this.DbSet.Where(x => x.Homework_Id == hwId && x.Student_Id == studentId && x.IsDeleted == false).FirstOrDefault();
        }
    }
}
