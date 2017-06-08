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

    public class AdministratorService : IAdministratorService
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
            if (administrator == null)
                return new MichtavaFailure("חייב לספק אובייקט ליצירה...");


            if (administrator.ApplicationUser == null && administrator.ApplicationUserId == null)
            {
                return new MichtavaFailure("must attach ApplicationUser before creation.");
            }

            //if (administrator.ApplicationUser.UserName == null || administrator.ApplicationUser.UserName == "")
            //    return new MichtavaFailure("חובה להזין שם משתמש.");


            //if (userRepository.Get(x => x.UserName == administrator.ApplicationUser.UserName).FirstOrDefault() == null)
            //    return new MichtavaFailure("please add ApplicationUser before using this function");



            //ApplicationUser user = administrator.ApplicationUser;
            //administrator.ApplicationUser = null;
            //administrator.ApplicationUserId = null;
                      
           

            this.administratorRepository.Add(administrator);
            this.administratorRepository.SaveChanges();

            //administrator.ApplicationUser = user;

            //return Update(administrator);
            return new MichtavaSuccess();

        }

        public MichtavaResult Update(Administrator administrator)
        {

            if (administrator.ApplicationUser == null)
            {
                return new MichtavaFailure("must attach ApplicationUser.");
            }

            if (administrator.ApplicationUser.UserName == null || administrator.ApplicationUser.UserName == "")
                return new MichtavaFailure("חובה להזין שם משתמש.");

            if (userRepository.Get(sc => sc.UserName == administrator.ApplicationUser.UserName).Count() == 1 &&
                                              userRepository.Get(sc => sc.UserName == administrator.ApplicationUser.UserName).
                                              FirstOrDefault().Id != administrator.ApplicationUser.Id)
                return new MichtavaFailure("לא ניתן לשנות את פרטי המשתמש - שם המשתמש כבר קיים");


            

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




            this.administratorRepository.Delete(administrator);
            this.administratorRepository.SaveChanges();
            //this.userRepository.Delete(administrator.ApplicationUser);

            //this.userRepository.SaveChanges();
            return new MichtavaSuccess("משתמש נמחק בהצלחה");
        }

        public MichtavaResult HardDelete(Administrator administrator)
        {

            Administrator existing = this.administratorRepository.AllWithDeleted().Where(y => y.Id == administrator.Id).
               FirstOrDefault();


            if (existing == null)
                return new MichtavaFailure("המשתמש לא נמצא במערכת");




            this.administratorRepository.HardDelete(administrator);
            this.administratorRepository.SaveChanges();
            //this.userRepository.HardDelete(administrator.ApplicationUser);

            //this.userRepository.SaveChanges();
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
