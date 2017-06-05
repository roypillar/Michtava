namespace Services
{
    using System;
    using System.Linq;
    using Dal.Repositories.Interfaces;
    using Entities.Models;
    using Services.Interfaces;
    using Common;
    using System.Data.Entity;
    using System.Collections.Generic;

    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository AnswerRepository;

        public AnswerService(IAnswerRepository answerRepository)
        {
            this.AnswerRepository = answerRepository;
        }



        public IQueryable<Answer> All()
        {
            return this.AnswerRepository.All();
        }

        public Answer GetById(Guid id)
        {
            return this.AnswerRepository.GetById(id);
        }

        public Answer GetByDetails(Guid hwId, Guid studentId)
        {
            return this.AnswerRepository.Get(ans => ans.Homework_Id == hwId && ans.Student_Id == studentId && !ans.IsDeleted).FirstOrDefault();
        }

        public MichtavaResult Add(Answer Answer)
        {
           if(Answer.Submitted_By ==null)
                return new MichtavaFailure("התשובה חייבת להכיל את שיעורי הבית");


            if (Answer.Answer_To == null)
                return new MichtavaFailure("התשובה חייבת להכיל את הסטודנט המגיש");

            if(AnswerRepository.Get(x => x.Homework_Id == Answer.Homework_Id &&
                                         x.Student_Id == Answer.Student_Id).Count() != 0)
                return new MichtavaFailure("תשובה עם אותו התלמיד ואותם השיעורים כבר קיימת במערכת");


            this.AnswerRepository.Add(Answer);
            this.AnswerRepository.SaveChanges();
            return new MichtavaSuccess("תשובה נוספה בהצלחה");
        }

        public MichtavaResult Update(Answer Answer)
        {
            if (this.GetById(Answer.Id) == null)
                return new MichtavaFailure("התשובה לא נמצאה במערכת");


            if (Answer.Submitted_By == null)
                return new MichtavaFailure("התשובה חייבת להכיל את שיעורי הבית");


            if (Answer.Answer_To == null)
                return new MichtavaFailure("התשובה חייבת להכיל את הסטודנט המגיש");

          


            this.AnswerRepository.Update(Answer);
            this.AnswerRepository.SaveChanges();
            return new MichtavaSuccess("תשובה עודכנה בהצלחה");
        }


        public MichtavaResult Delete(Answer Answer)
        {
            Answer existing = this.AnswerRepository.All().Where(y => y.Id == Answer.Id).FirstOrDefault();
               


            if (existing == null)
                return new MichtavaFailure("התשובה לא נמצאה במערכת");


            this.AnswerRepository.Delete(Answer);   
            this.AnswerRepository.SaveChanges();



            return new MichtavaSuccess("תשובה נמחקה בהצלחה");
        }



        public MichtavaResult HardDelete(Answer Answer)
        {
            Answer existing = this.AnswerRepository.AllWithDeleted().Where(y => y.Id == Answer.Id).FirstOrDefault();



            if (existing == null)
                return new MichtavaFailure("התשובה לא נמצאה במערכת");


            this.AnswerRepository.HardDelete(Answer);
            this.AnswerRepository.SaveChanges();

            return new MichtavaSuccess("תשובה נמחקה בהצלחה");
        }

    }
}
