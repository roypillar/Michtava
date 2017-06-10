namespace Services
{
    using System.Linq;
    using Dal.Repositories.Interfaces;
    using Entities.Models;
    using Services.Interfaces;
    using System;
    using Common;

    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository subjectRepository;

        public SubjectService(ISubjectRepository subjectRepository)
        {
            this.subjectRepository = subjectRepository;
        }

        public Subject GetById(Guid id)
        {
            return this.subjectRepository.GetById(id);
        }


        public Subject GetByName(string subjectName)
        {
            return this.subjectRepository.GetByName(subjectName);
        }

        public IQueryable<Subject> All()
        {
            return this.subjectRepository.All();
        }

        public MichtavaResult Add(Subject subject)
        {
            if (subjectRepository.GetByName(subject.Name) != null)
                return new MichtavaFailure("כבר קיים נושא עם אותו השם");



            this.subjectRepository.Add(subject);
            this.subjectRepository.SaveChanges();
            return new MichtavaSuccess();

        }

        public MichtavaResult Update(Subject subject)
        {
            if (this.GetById(subject.Id) == null)
                return new MichtavaFailure("הנושא לא נמצא במערכת");

            if (subjectRepository.Get(s => s.Name == subject.Name).Count() > 1)
                return new MichtavaFailure();

            if (subjectRepository.Get(s => s.Name == subject.Name).Count() == 1 &&
                subjectRepository.Get(s => s.Name == subject.Name).FirstOrDefault().Id == subject.Id)
                return new MichtavaFailure("כבר קיים נושא במערכת עם אותו השם");


            if (subjectRepository.Get(s => s.Name == subject.Name).Count() == 0)
            {
                this.subjectRepository.Update(subject);
                this.subjectRepository.SaveChanges();
                return new MichtavaSuccess("נושא עודכן בהצלחה");
            }
            else
                return new MichtavaFailure("טעות לא צפויה התרחשה");
        }

        public MichtavaResult Delete(Subject subject)
        {
            Subject existing = this.subjectRepository.All().Where(y => y.Id == subject.Id).FirstOrDefault();


            if (existing == null)
                return new MichtavaFailure("הנושא לא נמצא במערכת");

            //delete from all texts and scs...???

            this.subjectRepository.Delete(subject);
            this.subjectRepository.SaveChanges();
            return new MichtavaSuccess("הנושא נמחק בהצלחה");

        }

        public MichtavaResult HardDelete(Subject subject)
        {
            Subject existing = this.subjectRepository.AllWithDeleted().Where(y => y.Id == subject.Id).FirstOrDefault();


            if (existing == null)
                return new MichtavaFailure("הנושא לא נמצא במערכת");

            //delete from all ss and scs...???


            this.subjectRepository.HardDelete(subject);
            this.subjectRepository.SaveChanges();
            return new MichtavaSuccess("הנושא נמחק בהצלחה");

        }
    }
}
