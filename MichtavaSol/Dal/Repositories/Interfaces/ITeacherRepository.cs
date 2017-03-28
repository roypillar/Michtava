namespace Dal.Repositories.Interfaces
{
    using System.Linq;
    using System.Threading.Tasks;
    using Entities.Models;

    public interface ITeacherRepository : IDeletableEntityRepository<Teacher>
    {
        Task<Teacher> GetByUserName(string username);

        IQueryable<Teacher> SearchByName(string searchString);

        bool IsUserNameUniqueOnEdit(Teacher teacher, string username);
    }
}
