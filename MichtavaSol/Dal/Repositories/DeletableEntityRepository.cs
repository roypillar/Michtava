namespace Dal.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Entities.Models;
    using Dal.Repositories.Interfaces;

    public class DeletableEntityRepository<T> : GenericRepository<T>, IDeletableEntityRepository<T> where T :
        class, IDeletableEntity
    {
        public DeletableEntityRepository(IApplicationDbContext context) : base(context)
        {
        }

        public override IQueryable<T> All()
        {
            return base.All().Where(x => !x.IsDeleted);
        }

        public IQueryable<T> AllWithDeleted()
        {
            return base.All();
        }

        public override void Delete(T entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.Now;
            this.ChangeEntityState(entity, EntityState.Modified);
        }

        public void HardDelete(T entity)
        {
            base.Delete(entity);
        }

        public T GetByTestID (int Id)//FOR TESTING ONLY
        {
            return this.DbSet.Local.Where(e => e.TestID == Id).FirstOrDefault();
        }
    }
}
