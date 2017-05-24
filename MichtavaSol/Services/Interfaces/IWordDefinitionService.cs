namespace Services.Interfaces
{
    using Common;
    using Entities.Models;
    using System;

    public interface IWordDefinitionService : IRepositoryService<WordDefinition>
    {
        WordDefinition GetByWord(string w);

        MichtavaResult HardDelete(WordDefinition wd);
    }
}
