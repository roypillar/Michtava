namespace Services
{
    using System;
    using System.Linq;
    using Dal.Repositories.Interfaces;
    using Entities.Models;
    using Services.Interfaces;
    using Common;

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

        public MichtavaResult Add(Answer answer)
        {
            this.answerRepository.Add(answer);
            this.answerRepository.SaveChanges();
            return new MichtavaSuccess();
        }

        public MichtavaResult Update(Answer answer)
        {
            this.answerRepository.Update(answer);

            this.answerRepository.SaveChanges();
            return new MichtavaSuccess();

        }

        public MichtavaResult Delete(Answer answer)
        {

            this.answerRepository.Delete(answer);
            this.answerRepository.SaveChanges();
            return new MichtavaSuccess();

        }
    }
}
