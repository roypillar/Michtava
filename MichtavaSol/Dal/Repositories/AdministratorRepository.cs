namespace Dal.Repositories
{
    using Entities.Models;
    using Dal.Repositories.Interfaces;

    public class AdministratorRepository : DeletableEntityRepository<Administrator>, IAdministratorRepository
    {
        public AdministratorRepository(IApplicationDbContext context) : base(context)
        {
        }
    }
}
