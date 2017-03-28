namespace Dal.Repositories
{
    using Entities.Models;
    using Dal.Repositories.Interfaces;

    public class SubjectRepository : DeletableEntityRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(IApplicationDbContext context) : base(context)
        {
        }
    }
}
