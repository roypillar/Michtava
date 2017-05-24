namespace Dal.Repositories
{
    using Entities.Models;
    using Dal.Repositories.Interfaces;
    using System;
    using System.Linq;

    public class TextRepository : DeletableEntityRepository<Text>, ITextRepository
    {
        public TextRepository(IApplicationDbContext context) : base(context)
        {
         
        }

        public Text GetByName(string name)
        {
            var t = this.DbSet
                .Where(s => s.Name.Equals(name) && s.IsDeleted == false).FirstOrDefault();

            return t;
        }
    }
}
