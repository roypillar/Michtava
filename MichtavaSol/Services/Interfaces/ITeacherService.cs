namespace Services.Interfaces
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Dal.Repositories.Interfaces;
    using Entities.Models;

    public interface ITeacherService : IRepositoryService<Teacher>
    {
        IApplicationUserRepository UserRepository { get; }

        Teacher GetById(Guid id);

        IQueryable<Teacher> SearchByName(string searchString);

        Task<Teacher> GetByUserName(string username);

        bool IsUserNameUniqueOnEdit(Teacher teacher, string username);
    }
}
