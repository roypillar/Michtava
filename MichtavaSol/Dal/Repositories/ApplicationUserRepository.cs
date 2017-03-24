namespace Dal.Repositories
{
    using Entities.Models;
    using Dal.Repositories.Interfaces;

    public class ApplicationUserRepository : DeletableEntityRepository<ApplicationUser> , IApplicationUserRepository
    {
        private readonly IApplicationDbContext context;

        public ApplicationUserRepository(IApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public ApplicationDbContext Context
        {
            get { return (ApplicationDbContext)this.context; }
        }
    }
}