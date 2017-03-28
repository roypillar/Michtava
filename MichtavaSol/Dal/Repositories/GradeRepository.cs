namespace Dal.Repositories
{
    using Dal.Repositories.Interfaces;
    using Entities.Models;

    public class GradeRepository : DeletableEntityRepository<Grade>, IGradeRepository
    {
        public GradeRepository(IApplicationDbContext context) : base(context)
        {
        }
    }
}
