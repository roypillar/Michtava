namespace Dal
{
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Migrations;
    using Entities.Models;
    using System.Collections.Generic;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        public IDbSet<Administrator> Administrators { get; set; }

        public IDbSet<Text> Texts { get; set; }

        public IDbSet<Student> Students { get; set; }

        public IDbSet<Teacher> Teachers { get; set; }


        public IDbSet<Subject> Subjects { get; set; }

        public IDbSet<SchoolClass> SchoolClasses { get; set; }

        public IDbSet<Homework> Homeworks { get; set; }

        //public IDbSet<Grade> Grades { get; set; }

        public IDbSet<Answer> Answers { get; set; }


        public ICollection<Text> getTexts(string t)
        {
            return (
                    from text in Texts
                    where text.Name.Contains(t)
                    select text
                 ).ToList<Text>();
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public new void SaveChanges()
        {
            try
            {
                base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    // Do not pluralize the db tables
        //    modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        //    base.OnModelCreating(modelBuilder);


        //}
        // add-migration -ProjectName Dal -StartUpProject Frontend




        //enable-migrations -ContextProjectName Dal -StartUpProjectName Frontend -ContextTypeName Dal.ApplicationDbContext -ProjectName Dal -force

        //Update-Database -ConfigurationTypeName Dal.Migrations.Configuration -ProjectName Dal

        //

        //ONLY ONE TIME! (16.04.2017) : Update-Database -ConfigurationTypeName Dal.Migrations.Configuration -ProjectName Dal -TargetMigration test1604 -verbose




    }
}
