namespace Dal.Repositories
{
    using Entities.Models;
    using Dal.Repositories.Interfaces;

    public class HomeworkRepository : DeletableEntityRepository<Homework>, IHomeworkRepository
    {
        public HomeworkRepository(IApplicationDbContext context) : base(context)
        {
        }
    }
}
