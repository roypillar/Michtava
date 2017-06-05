using System.Threading.Tasks;

namespace Dal.Repositories
{
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Entities.Models;
    using Dal.Repositories.Interfaces;

    public class StudentRepository : DeletableEntityRepository<Student>, IStudentRepository
    {
        private readonly IApplicationDbContext context;

        public StudentRepository(IApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Student> GetByUserName(string username)
        {
            var store = new UserStore<ApplicationUser>((ApplicationDbContext)this.context);
            var userManager = new UserManager<ApplicationUser>(store);

            ApplicationUser user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                return null;
            }

            Student student = this.All().FirstOrDefault(s => s.ApplicationUserId == user.Id);

            return student;
        }

        public IQueryable<Student> SearchByName(string searchString)
        {
            // var query = this.Get(filter: student => student.Name.Contains(searchString));
            var query = this.All().Where(student => student.Name.Contains(searchString));

            return query;
        }

        public bool IsUserNameUniqueOnEdit(Student student, string username)
        {
            bool usernameUnique = !this.AllWithDeleted()
                .Any(s =>
                    (s.ApplicationUser.UserName == username) &&
                    (s.ApplicationUserId != student.ApplicationUserId));

            return usernameUnique;
        }

        public bool IsEmailUniqueOnEdit(Student student, string email)
        {
            bool emailUnique = !this.AllWithDeleted()
                .Any(s =>
                    (s.ApplicationUser.Email == email) &&
                    (s.ApplicationUserId != student.ApplicationUserId));

            return emailUnique;
        }
    }
}