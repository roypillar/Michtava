namespace Dal.Repositories
{
    using Entities.Models;
    using Dal.Repositories.Interfaces;
    using System;
    using System.Linq;

    public class SubjectRepository : DeletableEntityRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(IApplicationDbContext context) : base(context)
        {
        }

        public Subject GetByName(string subjectName)
        {
            
            var sub = this.DbSet
                  .Where(s => s.Name.Equals(subjectName)).FirstOrDefault();

            return sub;
        }
    }
}
