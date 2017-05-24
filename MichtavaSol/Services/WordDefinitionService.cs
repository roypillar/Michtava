namespace Services
{
    using System.Linq;
    using Dal.Repositories.Interfaces;
    using Entities.Models;
    using Services.Interfaces;
    using System;
    using Common;

    public class WordDefinitionService : IWordDefinitionService
    {
        private readonly IWordDefinitionRepository WordDefinitionRepository;

        public WordDefinitionService(IWordDefinitionRepository WordDefinitionRepository)
        {
            this.WordDefinitionRepository = WordDefinitionRepository;
        }

        public WordDefinition GetById(Guid id)
        {
            return this.WordDefinitionRepository.GetById(id);
        }


        public WordDefinition GetByWord(string w)
        {
            return this.WordDefinitionRepository.GetByWord(w);
        }

        public IQueryable<WordDefinition> All()
        {
            return this.WordDefinitionRepository.All();
        }

        public MichtavaResult Add(WordDefinition WordDefinition)
        {
            if (WordDefinitionRepository.GetByWord(WordDefinition.Word) != null)
                return new MichtavaFailure("כבר קיימת מילה כזאת במערכת");



            this.WordDefinitionRepository.Add(WordDefinition);
            this.WordDefinitionRepository.SaveChanges();
            return new MichtavaSuccess();

        }


        public MichtavaResult Update(WordDefinition WordDefinition)
        {
            if (this.GetByWord(WordDefinition.Word) == null)
                return new MichtavaFailure("המילה לא נמצאה במערכת");

           
                this.WordDefinitionRepository.Update(WordDefinition);
                this.WordDefinitionRepository.SaveChanges();
                return new MichtavaSuccess("מילה עודכנה בהצלחה");
         
        }

        public MichtavaResult Delete(WordDefinition WordDefinition)
        {
            WordDefinition existing = this.WordDefinitionRepository.All().Where(y => y.Word == WordDefinition.Word).FirstOrDefault();


            if (existing == null)
                return new MichtavaFailure("המילה לא נמצאה במערכת");


            this.WordDefinitionRepository.Delete(WordDefinition);
            this.WordDefinitionRepository.SaveChanges();
            return new MichtavaSuccess("המילה נמחקה בהצלחה");

        }

        public MichtavaResult HardDelete(WordDefinition WordDefinition)
        {
            WordDefinition existing = this.WordDefinitionRepository.AllWithDeleted().Where(y => y.Word == WordDefinition.Word).FirstOrDefault();


            if (existing == null)
                return new MichtavaFailure("המילה לא נמצאה במערכת");


            this.WordDefinitionRepository.HardDelete(WordDefinition);
            this.WordDefinitionRepository.SaveChanges();
            return new MichtavaSuccess("המילה נמחקה בהצלחה");

        }
    }
}
