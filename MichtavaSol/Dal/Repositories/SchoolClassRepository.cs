namespace Dal.Repositories
{
    using System.Linq;
    using Entities.Models;
    using Interfaces;

    public class SchoolClassRepository : DeletableEntityRepository<SchoolClass>, ISchoolClassRepository
    {
        public SchoolClassRepository(IApplicationDbContext context) : base(context)
        {
        }

        public SchoolClass GetByDetails(int classNumber, string letter)
        {
            if (classNumber <= 0 || classNumber > 100 || letter.Length != 1)
                return null;//robust this thing up

            SchoolClass schoolClass = this.All().
                Where(sc => sc.ClassNumber == classNumber &&
                         sc.ClassLetter == letter)
                .FirstOrDefault();
            return schoolClass;
        }
    }
}
