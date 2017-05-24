namespace Dal.Repositories
{
    using Entities.Models;
    using Dal.Repositories.Interfaces;
    using System;
    using System.Linq;

    public class WordDefinitionRepository : DeletableEntityRepository<WordDefinition>, IWordDefinitionRepository
    {
        public WordDefinitionRepository(IApplicationDbContext context) : base(context)
        {

        }

        public WordDefinition GetByWord(string w)
        {
            var t = this.DbSet
                .Where(s => s.Word.Equals(w) && s.IsDeleted == false).FirstOrDefault();

            return t;
        }
    }
}
