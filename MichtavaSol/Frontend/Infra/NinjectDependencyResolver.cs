namespace Frontend.Infra
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Ninject;
    using Ninject.Web.Common;
    using Dal;
    using Dal.Repositories;
    using Dal.Repositories.Interfaces;
    using Services;
    using Services.Interfaces;

    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            this.kernel = kernelParam;
            this.AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return this.kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            this.kernel.Bind<IApplicationDbContext>().To<ApplicationDbContext>().InRequestScope();

            /*kernel.Bind<IUserStore<ApplicationUser>>()
               .To<UserStore<ApplicationUser>>()
               .WithConstructorArgument("applicationDbContext", new ApplicationDbContext());*/


            //bind all repository interfaces to actual implementations here

            this.kernel.Bind<ITextRepository>().To<TextRepository>();
            this.kernel.Bind<IAnswerRepository>().To<AnswerRepository>();
            this.kernel.Bind<IHomeworkRepository>().To<HomeworkRepository>();
            this.kernel.Bind<ISchoolClassRepository>().To<SchoolClassRepository>();
            this.kernel.Bind<ISubjectRepository>().To<SubjectRepository>();
            this.kernel.Bind<IAdministratorRepository>().To<AdministratorRepository>();
            this.kernel.Bind<IStudentRepository>().To<StudentRepository>();
            this.kernel.Bind<ITeacherRepository>().To<TeacherRepository>();
            this.kernel.Bind<IApplicationUserRepository>().To<ApplicationUserRepository>();


            //bind all service interfaces to actual implementations here

            this.kernel.Bind<ITextService>().To<TextService>();
            this.kernel.Bind<IAnswerService>().To<AnswerService>();
            this.kernel.Bind<IHomeworkService>().To<HomeworkService>();
        
            this.kernel.Bind<ISchoolClassService>().To<SchoolClassService>();
            this.kernel.Bind<ISubjectService>().To<SubjectService>();
            this.kernel.Bind<IAdministratorService>().To<AdministratorService>();
            this.kernel.Bind<IStudentService>().To<StudentService>();
            this.kernel.Bind<ITeacherService>().To<TeacherService>();
        }
    }
}