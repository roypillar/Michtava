namespace Services
{
    using System;
    using System.Linq;
    using Dal.Repositories.Interfaces;
    using Entities.Models;
    using Services.Interfaces;

    public class HomeworkService : IHomeworkService
    {
        private readonly IHomeworkRepository homeworkRepository;

        public HomeworkService(IHomeworkRepository homeworkRepository)
        {
            this.homeworkRepository = homeworkRepository;
        }

        Homework IHomeworkService.GetById(Guid id)
        {
            return this.homeworkRepository.GetById(id);
        }

        IQueryable<Homework> IRepositoryService<Homework>.All()
        {
            return this.homeworkRepository.All();
        }

        public void Add(Homework homework)
        {
            this.homeworkRepository.Add(homework);
            this.homeworkRepository.SaveChanges();
        }

        public void Update(Homework homework)
        {
            this.homeworkRepository.Update(homework);
            this.homeworkRepository.SaveChanges();
        }

        public void Delete(Homework homework)
        {

            this.homeworkRepository.Delete(homework);
            this.homeworkRepository.SaveChanges();
        }
    }
}
