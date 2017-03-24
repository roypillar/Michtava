namespace Dal.Repositories.Interfaces
{
    using Entities.Models;

    public interface IApplicationUserRepository : IDeletableEntityRepository<ApplicationUser>
    {
        ApplicationDbContext Context { get; }
    }
}
