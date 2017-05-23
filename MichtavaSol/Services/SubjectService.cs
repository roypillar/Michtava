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
            this.subjectRepository.Add(subject);
            this.subjectRepository.SaveChanges();
            return new MichtavaSuccess();

        }

        public MichtavaResult Update(Subject subject)
        {
            this.subjectRepository.Update(subject);
            this.subjectRepository.SaveChanges();
            return new MichtavaSuccess();

        }

        public MichtavaResult Delete(Subject subject)
        {
            this.subjectRepository.Delete(subject);
            this.subjectRepository.SaveChanges();
            return new MichtavaSuccess();

        }

        public MichtavaResult HardDelete(Subject subject)
        {
            this.subjectRepository.HardDelete(subject);
            this.subjectRepository.SaveChanges();
            return new MichtavaSuccess();
        }
    }
}
