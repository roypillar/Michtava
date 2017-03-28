namespace Dal.Repositories.Interfaces
{
    using System.Linq;
    using Entities.Models;
    using System.Threading.Tasks;


    public interface IStudentRepository : IDeletableEntityRepository<Student>
    {
        Task<Student> GetByUserName(string username);

        IQueryable<Student> SearchByName(string searchString);

        bool IsUserNameUniqueOnEdit(Student student, string username);

        bool IsEmailUniqueOnEdit(Student student, string email);
    }
}