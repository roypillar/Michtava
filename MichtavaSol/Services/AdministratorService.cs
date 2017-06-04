namespace Services
{
    using System;
    using System.Linq;
    using Dal.Repositories;
    using Dal.Repositories.Interfaces;
    using Entities.Models;
    using Services.Interfaces;
    using Microsoft.AspNet.Identity;
    using Common;

    public class AdministratorService :IAdministratorService
    {
        private readonly IAdministratorRepository administratorRepository;

        private readonly IApplicationUserRepository userRepository;

        public AdministratorService(
            IAdministratorRepository administratorRepository,
            IApplicationUserRepository userRepository)
        {
            this.administratorRepository = administratorRepository;
            this.userRepository = userRepository;
        }

        public IApplicationUserRepository UserRepository
        {
            get { return this.userRepository; }
        }

        public Administrator GetById(Guid id)
        {
            return this.administratorRepository.GetById(id);
        }

        public Administrator GetByUserName(string username)
        {
            return this.administratorRepository.All().FirstOrDefault(a => a.ApplicationUser.UserName == username && !a.IsDeleted);
        }

        public IQueryable<Administrator> All()
        {
            return this.administratorRepository.All();
        }

        public MichtavaResult Add(Administrator administrator)
        {
            if (userRepository.Get(x => x.UserName == administrator.ApplicationUser.UserName).Count() != 0)
                return new MichtavaFailure("קיים כבר משתמש עם אותו שם המשתמש");

            if(administrator.ApplicationUser == null)
            {
                return new MichtavaFailure("must attach ApplicationUser before creation.");
            }

            if( administrator.ApplicationUser.UserName==null || administrator.ApplicationUser.UserName=="")
                return new MichtavaFailure("חובה להזין שם משתמש.");


            this.administratorRepository.Add(administrator);
            this.administratorRepository.SaveChanges();
            return new MichtavaSuccess("משתמש נוסף בהצלחה");

        }

        public MichtavaResult Update(Administrator administrator)
        {
           

                if (userRepository.Get(sc =>  sc.UserName == administrator.ApplicationUser.UserName).Count() == 1 &&
                                                  userRepository.Get(sc => sc.UserName == administrator.ApplicationUser.UserName).
                                                  FirstOrDefault().Id != administrator.ApplicationUser.Id)
                    return new MichtavaFailure("לא ניתן לשנות את פרטי המשתמש - שם המשתמש כבר קיים");


            if (administrator.ApplicationUser.UserName == null || administrator.ApplicationUser.UserName == "")
                return new MichtavaFailure("חובה להזין שם משתמש.");

            this.administratorRepository.Update(administrator);
            this.administratorRepository.SaveChanges();
            return new MichtavaSuccess("משתמש עודכן בהצלחה");

        }

        public MichtavaResult Delete(Administrator administrator)
        {

            Administrator existing = this.administratorRepository.All().Where(y => y.Id == administrator.Id).
               FirstOrDefault();


            if (existing == null)
                return new MichtavaFailure("המשתמש לא נמצא במערכת");



            administrator.ApplicationUser.DeletedBy = administrator.DeletedBy;

            this.administratorRepository.Delete(administrator);
            this.administratorRepository.SaveChanges();
            this.userRepository.Delete(administrator.ApplicationUser);

            this.userRepository.SaveChanges();
            return new MichtavaSuccess("משתמש נמחק בהצלחה");
        }

        public MichtavaResult HardDelete(Administrator administrator)
        {

            Administrator existing = this.administratorRepository.AllWithDeleted().Where(y => y.Id == administrator.Id).
               FirstOrDefault();


            if (existing == null)
                return new MichtavaFailure("המשתמש לא נמצא במערכת");



            administrator.ApplicationUser.DeletedBy = administrator.DeletedBy;

            this.administratorRepository.HardDelete(administrator);
            this.administratorRepository.SaveChanges();
            this.userRepository.HardDelete(administrator.ApplicationUser);

            this.userRepository.SaveChanges();
            return new MichtavaSuccess("משתמש נמחק בהצלחה");
        }

        public bool IsUserNameUniqueOnEdit(Administrator administrator, string username)
        {
            bool usernameUnique = !this.administratorRepository.AllWithDeleted()
                .Any(a => (a.ApplicationUser.UserName == username) &&
                    (a.ApplicationUserId != administrator.ApplicationUserId));

            return usernameUnique;
        }

        
    }
}
