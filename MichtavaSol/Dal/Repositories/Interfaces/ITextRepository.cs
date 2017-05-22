namespace Dal.Repositories.Interfaces
{
    using Entities.Models;

    public interface ITextRepository : IDeletableEntityRepository<Text>
    {

        Text GetByName(string name);
    }
}
