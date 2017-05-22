namespace Services
{
    using System;
    using System.Linq;
    using Dal.Repositories.Interfaces;
    using Entities.Models;
    using Services.Interfaces;
    using Common;

    public class TextService : ITextService
    {
        private readonly ITextRepository TextRepository;

        public TextService(ITextRepository TextRepository)
        {
            this.TextRepository = TextRepository;
        }

        Text ITextService.GetById(Guid id)
        {
            return this.TextRepository.GetById(id);
        }

        IQueryable<Text> IRepositoryService<Text>.All()
        {
            return this.TextRepository.All();
        }

        public MichtavaResult Add(Text Text)
        {
            this.TextRepository.Add(Text);
            this.TextRepository.SaveChanges();
            return new MichtavaSuccess();
        }

        public MichtavaResult Update(Text Text)
        {
            this.TextRepository.Update(Text);
            this.TextRepository.SaveChanges();
            return new MichtavaSuccess();

        }

        public MichtavaResult Delete(Text Text)
        {

            this.TextRepository.Delete(Text);
            this.TextRepository.SaveChanges();
            return new MichtavaSuccess();

        }



    }
}
