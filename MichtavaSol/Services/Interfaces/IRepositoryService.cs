namespace Services.Interfaces
{
    using Common;
    using System;
    using System.Linq;

    public interface IRepositoryService<T>
    {
        IQueryable<T> All();

        MichtavaResult Add(T entity);

        MichtavaResult Update(T entity);

        MichtavaResult Delete(T entity);
    }
}