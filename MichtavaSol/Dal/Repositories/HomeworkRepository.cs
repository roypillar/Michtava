namespace Dal.Repositories
{
    using Entities.Models;
    using Dal.Repositories.Interfaces;
    using System;
    using System.Linq;
    using Common.Exceptions;
    public class HomeworkRepository : DeletableEntityRepository<Homework>, IHomeworkRepository
    {
        public HomeworkRepository(IApplicationDbContext context) : base(context)
        {
        }

        public Homework GetByDetails(string title, string description)
        {
            var h = this.All().Where(hw => hw.Title == title && hw.Description == description && hw.IsDeleted == false);

            if (h.Count() == 0)
            {
                return null;
            }

            else if (h.Count() > 1)
                throw new MoreThanOneElementException("there are more than 1 homeworks matching that description.");
                
            return h.FirstOrDefault();
        }
    }
}
