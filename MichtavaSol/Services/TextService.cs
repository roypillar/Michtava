namespace Services
{
    using System.Linq;
    using Dal.Repositories.Interfaces;
    using Entities.Models;
    using Services.Interfaces;
    using System;
    using Common;

    public class TextService : ITextService
    {
        private readonly ITextRepository TextRepository;

        public TextService(ITextRepository TextRepository)
        {
            this.TextRepository = TextRepository;
        }

        public Text GetById(Guid id)
        {
            return this.TextRepository.GetById(id);
        }


        public Text GetByName(string TextName)
        {
            return this.TextRepository.GetByName(TextName);
        }

        public IQueryable<Text> All()
        {
            return this.TextRepository.All();
        }

        public MichtavaResult Add(Text Text)
        {
            if (TextRepository.GetByName(Text.Name) != null)
                return new MichtavaFailure("כבר קיים טקסט עם אותו השם");



            this.TextRepository.Add(Text);
            this.TextRepository.SaveChanges();
            return new MichtavaSuccess();

        }

        public MichtavaResult Update(Text Text)
        {
            if (this.GetById(Text.Id) == null)
                return new MichtavaFailure("הטקסט לא נמצא במערכת");

            if (TextRepository.Get(s => s.Name == Text.Name).Count() > 1)
                return new MichtavaFailure();

            if (TextRepository.Get(s => s.Name == Text.Name).Count() == 1)
                return new MichtavaFailure("כבר קיים טקסט במערכת עם אותו השם");

            if (TextRepository.Get(s => s.Name == Text.Name).Count() == 0)
            {
                this.TextRepository.Update(Text);
                this.TextRepository.SaveChanges();
                return new MichtavaSuccess("טקסט עודכן בהצלחה");
            }
            else
                return new MichtavaFailure("טעות לא צפויה התרחשה");
        }

        public MichtavaResult Delete(Text Text)
        {
            Text existing = this.TextRepository.All().Where(y => y.Id == Text.Id).FirstOrDefault();


            if (existing == null)
                return new MichtavaFailure("הטקסט לא נמצא במערכת");

            //delete from all hws???

            this.TextRepository.Delete(Text);
            this.TextRepository.SaveChanges();
            return new MichtavaSuccess("הטקסט נמחק בהצלחה");

        }

        public MichtavaResult HardDelete(Text Text)
        {
            Text existing = this.TextRepository.AllWithDeleted().Where(y => y.Id == Text.Id).FirstOrDefault();


            if (existing == null)
                return new MichtavaFailure("הטקסט לא נמצא במערכת");


            this.TextRepository.HardDelete(Text);
            this.TextRepository.SaveChanges();
            return new MichtavaSuccess("הטקסט נמחק בהצלחה");

        }
    }
}
