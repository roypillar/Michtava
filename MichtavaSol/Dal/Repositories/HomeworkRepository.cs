namespace Dal.Repositories
{
    using Entities.Models;
    using Dal.Repositories.Interfaces;
    using System;
    using System.Linq;

    public class HomeworkRepository : DeletableEntityRepository<Homework>, IHomeworkRepository
    {
        public HomeworkRepository(IApplicationDbContext context) : base(context)
        {
        }

        public Homework GetByDetails(string Title)
        {
            if (Title == null) 
                return null;

            Homework h = this.All().Where(hw => hw.Title == Title && hw.IsDeleted == false)
                .FirstOrDefault();
            return h;
        }
    }
}
