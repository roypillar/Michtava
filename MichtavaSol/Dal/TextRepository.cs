namespace Dal.Repositories
{
    using Entities.Models;
    using Dal.Repositories.Interfaces;

    public class TextRepository : DeletableEntityRepository<Text>, ITextRepository
    {
        public TextRepository(IApplicationDbContext context) : base(context)
        {
        }
    }
}
