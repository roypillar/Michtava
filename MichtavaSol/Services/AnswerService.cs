namespace Services
{
    using System;
    using System.Linq;
    using Dal.Repositories.Interfaces;
    using Entities.Models;
    using Services.Interfaces;

    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository answerRepository;

        public AnswerService(IAnswerRepository answerRepository)
        {
            this.answerRepository = answerRepository;
        }

        Answer IAnswerService.GetById(Guid id)
        {
            return this.answerRepository.GetById(id);
        }

        IQueryable<Answer> IRepositoryService<Answer>.All()
        {
            return this.answerRepository.All();
        }

        public void Add(Answer answer)
        {
            this.answerRepository.Add(answer);
            this.answerRepository.SaveChanges();
        }

        public void Update(Answer answer)
        {
            this.answerRepository.Update(answer);
            this.answerRepository.SaveChanges();
        }

        public void Delete(Answer answer)
        {

            this.answerRepository.Delete(answer);
            this.answerRepository.SaveChanges();
        }
    }
}
