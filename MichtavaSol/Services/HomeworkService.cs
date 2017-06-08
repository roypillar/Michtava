namespace Services
{
    using System;
    using System.Linq;
    using Dal.Repositories.Interfaces;
    using Entities.Models;
    using Services.Interfaces;
    using Common;

    public class HomeworkService : IHomeworkService
    {
        private readonly IHomeworkRepository homeworkRepository;

        public HomeworkService(IHomeworkRepository homeworkRepository)
        {
            this.homeworkRepository = homeworkRepository;
        }

        public IQueryable<Homework> All()
        {
            return this.homeworkRepository.All();
        }

       public Homework GetById(Guid id)
        {
            return this.homeworkRepository.GetById(id);
        }

        public Homework GetByDetails(string title, string description)
        {
            return this.homeworkRepository.GetByDetails(title,description);
        }

        public MichtavaResult Add(Homework homework)
        {
            if (homeworkRepository.GetByDetails(homework.Title, homework.Description) != null)
                return new MichtavaFailure("כבר קיימים שיעורי בית עם אותה הכותרת ואותו התיאור");

            if(homework.Created_By == null)
                return new MichtavaFailure("חייב לציין מי המורה שיצר\\ה את שיעורי הבית.");

            if (homework.Text == null)
                return new MichtavaFailure("חובה לציין את הטקסט עליו מבוססים שיעורי הבית.");

            if (homework.Deadline == null)
                homework.Deadline = DateTime.Now;

            this.homeworkRepository.Add(homework);
            this.homeworkRepository.SaveChanges();
            return new MichtavaSuccess("שיעורי בית נוספו בהצלחה");

        }


        public MichtavaResult Update(Homework homework)
        {

            if (this.GetById(homework.Id) == null)
                return new MichtavaFailure("השיעורים לא נמצאו במערכת");

            if (homework.Created_By == null)
                return new MichtavaFailure("חייב לציין מי המורה שיצר\\ה את שיעורי הבית.");

            if (homework.Text == null)
                return new MichtavaFailure("חובה לציין את הטקסט עליו מבוססים שיעורי הבית.");

            if (homework.Deadline == null)
                return new MichtavaFailure("אין למחוק את תאריך היצירה של השיעורים");

            if (homeworkRepository.Get(hw => hw.Title == homework.Title &&
                                               hw.Description == homework.Description).Count() > 1)
                return new MichtavaFailure();



            if (homeworkRepository.Get(hw => hw.Title == homework.Title &&
                                               hw.Description == homework.Description).Count() == 1 &&
                                            homeworkRepository.Get(hw => hw.Title == homework.Title &&
                                        hw.Description == homework.Description).FirstOrDefault().Id != homework.Id)
                return new MichtavaFailure("לא ניתן לשנות את פרטי השיעורים לפרטים שכבר מצויים במערכת, אצל שיעורים אחרים");


      

         
                this.homeworkRepository.Update(homework);
                this.homeworkRepository.SaveChanges();
                return new MichtavaSuccess("שיעורי בית עודכנו בהצלחה");
           


        }

        public MichtavaResult Delete(Homework homework)
        {
            Homework existing = this.homeworkRepository.All().Where(y => y.Id == homework.Id).FirstOrDefault();


            if (existing == null)
                return new MichtavaFailure("השיעורים לא נמצאו במערכת");

            //delete from all ss and scs...???


            this.homeworkRepository.Delete(homework);
            this.homeworkRepository.SaveChanges();
            return new MichtavaSuccess("שיעורי הבית נמחקו בהצלחה מהמערכת");


        }

         public MichtavaResult HardDelete(Homework homework)
        {
            Homework existing = this.homeworkRepository.AllWithDeleted().Where(y => y.Id == homework.Id).FirstOrDefault();


            if (existing == null)
                return new MichtavaFailure("השיעורים לא נמצאו במערכת");

            //delete from all ss and scs...???


            this.homeworkRepository.HardDelete(homework);
            this.homeworkRepository.SaveChanges();
            return new MichtavaSuccess("שיעורי הבית נמחקו בהצלחה מהמערכת");

        }
    }
}
