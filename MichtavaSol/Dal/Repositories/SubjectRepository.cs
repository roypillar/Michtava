﻿namespace Dal.Repositories
{
    using Entities.Models;
    using Dal.Repositories.Interfaces;
    using System;
    using System.Linq;
    using Common.Exceptions;
    public class SubjectRepository : DeletableEntityRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(IApplicationDbContext context) : base(context)
        {
        }

        public Subject GetByName(string subjectName)
        {
            
            var sub = this.DbSet
                  .Where(s => s.Name.Equals(subjectName) && s.IsDeleted == false).FirstOrDefault();

            return sub;
        }
      

        //OVERRIDE
        public new void Add(Subject subject)
        {
            if (this.GetByName(subject.Name) == null)
            {
                base.Add(subject);
            }
            else
                throw new NameDuplicateException("נושא עם אותו השם כבר קיים במערכת");
        }
    }
}
