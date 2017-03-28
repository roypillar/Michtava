namespace Services.Interfaces
{
    using Entities.Models;
    public interface ITextService : IRepositoryService<Text>
    {
        Text GetById(int id);
    }
}
