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
            SchoolClass schoolClass = this.All()
                .FirstOrDefault(
                    sc => sc.ClassNumber == classNumber &&
                          sc.ClassLetter == letter);
            return schoolClass;
        }
    }
}
