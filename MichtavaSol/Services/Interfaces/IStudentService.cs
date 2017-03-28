namespace Services.Interfaces
{
    using System.Linq;
    using System.Threading.Tasks;
    using Dal.Repositories.Interfaces;
    using Entities.Models;

    public interface IStudentService : IRepositoryService<Student>
    {
        IApplicationUserRepository UserRepository { get; }

        Student GetById(int id);

        IQueryable<Student> SearchByName(string searchString);

        Task<Student> GetByUserName(string username);

        bool IsUserNameUniqueOnEdit(Student student, string username);

        bool IsEmailUniqueOnEdit(Student student, string email);
    }
}