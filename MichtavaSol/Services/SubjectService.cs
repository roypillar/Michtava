namespace Services
{
    using System.Linq;
    using Dal.Repositories.Interfaces;
    using Entities.Models;
    using Services.Interfaces;
    using System;

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

        public void Add(Subject subject)
        {
            this.subjectRepository.Add(subject);
            this.subjectRepository.SaveChanges();
        }

        public void Update(Subject subject)
        {
            this.subjectRepository.Update(subject);
            this.subjectRepository.SaveChanges();
        }

        public void Delete(Subject subject)
        {
            this.subjectRepository.Delete(subject);
            this.subjectRepository.SaveChanges();
        }
    }
}
