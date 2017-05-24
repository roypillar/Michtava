namespace Dal.Repositories.Interfaces
{
    using Entities.Models;

    public interface IWordDefinitionRepository : IDeletableEntityRepository<WordDefinition>
    {
        WordDefinition GetByWord(string w);
    }
}
