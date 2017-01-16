namespace Dal
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    using Entities.Models;

    public interface IApplicationDbContext
    {
        IDbSet<ApplicationUser> Users { get; set; }

        void SaveChanges();

        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
