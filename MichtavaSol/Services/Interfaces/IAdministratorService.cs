namespace Services.Interfaces
{
    using System;
    using Dal.Repositories;
    using Dal.Repositories.Interfaces;

    using Entities.Models;
    using Common;

    public interface IAdministratorService : IRepositoryService<Administrator>
    {
        IApplicationUserRepository UserRepository { get; }

        Administrator GetById(Guid id);

        Administrator GetByUserName(string username);

        bool IsUserNameUniqueOnEdit(Administrator administrator, string username);

        MichtavaResult HardDelete(Administrator academicYear);
    }
}
