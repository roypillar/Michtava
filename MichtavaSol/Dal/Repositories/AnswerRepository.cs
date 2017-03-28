namespace Dal.Repositories
{
    using Entities.Models;
    using Dal.Repositories.Interfaces;

    public class AnswerRepository : DeletableEntityRepository<Answer>, IAnswerRepository
    {
        public AnswerRepository(IApplicationDbContext context) : base(context)
        {
        }
    }
}
