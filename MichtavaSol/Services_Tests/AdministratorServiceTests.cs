using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Entities.Models;
using System.Data.Common;
using Effort;
using Common;
using Dal;
using Services;
using Dal.Repositories;
using NUnit.Framework;
using NUnit;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;

namespace Services_Tests
{
    class AdministratorServiceTests
    {
        private ApplicationDbContext ctx;

        private AdministratorService serv;

        private Administrator entity;

        private ApplicationUser adminUser;

        private UserManager<ApplicationUser> userManager;

        private RoleManager<IdentityRole> roleManager;

        private const string USERNAME = "testerTestie";

        private const string fn = "מוטי";

        private const string ln = "מוטיבאום";

        [OneTimeSetUp]
        public void oneTimeSetUp()
        {
            var connection = DbConnectionFactory.CreateTransient();
            this.ctx = new ApplicationDbContext(connection);
            this.serv = new AdministratorService(new AdministratorRepository(ctx),
                                               new ApplicationUserRepository(ctx));
            var seeder = new DatabaseSeeder();
            seeder.CreateDependenciesAndSeed(ctx);//heavy duty
            this.userManager = seeder.userManager;
            this.roleManager = seeder.roleManager;

        }

        [SetUp]
        public void setUp()
        {
            adminUser = new ApplicationUser()
            {
                UserName = USERNAME,
                Email = "something@somewhere.com"
            };


            entity = new Administrator(adminUser,fn,ln);
        }

        //Adds
        [Test]
        public void testAddStandaloneAdministrator()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();


            // Act
            var result = this.userManager.Create(adminUser, "testpassword");

            if (result.Succeeded)
            {
                this.userManager.AddToRole(adminUser.Id, GlobalConstants.AdministratorRoleName);
            }
            MichtavaResult res = serv.Add(entity);

            // Assert
            Assert.AreEqual(count + 1, serv.All().Count());
            Assert.NotNull(serv.GetByUserName(USERNAME));
            Assert.True(res is MichtavaSuccess);
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
            this.userManager.Delete(adminUser);
            Assert.Null(serv.GetByUserName(USERNAME));
        }

        [Test]
        public void testAddAdministratorExistingId()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();
            Administrator existing = serv.All().FirstOrDefault();

            // Act
            
            Assert.Throws<DbEntityValidationException>(() => this.userManager.Create(existing.ApplicationUser, "testpassword"));
            oneTimeSetUp();

        }

        [Test]
        public void testAddExistingAdministratorDetails()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();
            Administrator existing = serv.All().FirstOrDefault();
            Administrator c = new Administrator(existing.ApplicationUser,existing.FirstName, existing.LastName);

            // Act
            Assert.Throws<DbEntityValidationException>(() => this.userManager.Create(existing.ApplicationUser, "testpassword"));
            oneTimeSetUp();


        }

        //Gets
        [Test]
        public void testGetAdministratorByIdTrue()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();

            Administrator c = serv.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Administrator actual = serv.GetById(c.Id);
            // Assert

            Assert.NotNull(actual);

        }

        [Test]
        public void testGetAdministratorByIdFalse()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();

            // Act
            Administrator actual = serv.GetById(Guid.NewGuid());
            // Assert

            Assert.Null(actual);

        }


        [Test]
        public void testGetAdministratorByDetailsTrue()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();

            Administrator c = serv.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Administrator actual = serv.GetByUserName(c.ApplicationUser.UserName);

            // Assert
            Assert.NotNull(actual);
        }



        [Test]
        public void testGetAdministratorByDetailsFalse()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();


            // Act
            Administrator actual = serv.GetByUserName("fsajklasjfkslajk");
            
            // Assert

            Assert.Null(actual);
        }

        //Update
        [Test]
        public void testUpdateAdministratorSuccess()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();

            var result = this.userManager.Create(adminUser, "testpassword");

            if (result.Succeeded)
            {
                this.userManager.AddToRole(adminUser.Id, GlobalConstants.AdministratorRoleName);
            }
            serv.Add(entity);

            Assert.Null(serv.GetByUserName("ichangedusername"));


            Assert.AreEqual(count + 1, serv.All().Count());


            // Act
            entity.ApplicationUser.UserName = "ichangedusername";
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.NotNull(serv.GetByUserName("ichangedusername"));
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
            this.userManager.Delete(adminUser);
            Assert.Null(serv.GetByUserName(USERNAME));
        }

        [Test]
        public void testUpdateAdministratorNonExistant()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();

            // Act
            MichtavaResult res = serv.Update(new Administrator() { FirstName = "גדע", LastName = "dsad" });

            // Assert
            Assert.True(res is MichtavaFailure);
        }

        [Test]
        public void testAddNullAppUser()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();
            Administrator existing = serv.All().FirstOrDefault();

            // Act
            Assert.Throws<DbEntityValidationException>(() => this.userManager.Create(existing.ApplicationUser, "testpassword"));
            oneTimeSetUp();
        }

        [Test]
        public void testUpdateAdministratorExistingDetails()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();
            Administrator c = serv.All().FirstOrDefault();
            var result = this.userManager.Create(adminUser, "testpassword");

            if (result.Succeeded)
            {
                this.userManager.AddToRole(adminUser.Id, GlobalConstants.AdministratorRoleName);
            }
            serv.Add(entity);

            Guid id = serv.GetByUserName(USERNAME).Id;

            Assert.AreEqual(count + 1, serv.All().Count());

            //Act
            entity.ApplicationUser.UserName = c.ApplicationUser.UserName;
            var res = serv.Update(entity);


            // Assert
            Assert.True(res is MichtavaFailure);
            entity.ApplicationUser.UserName = USERNAME;



            Assert.True(serv.GetById(id).ApplicationUser.UserName == USERNAME);
            Assert.True(serv.HardDelete(serv.GetById(id)) is MichtavaSuccess);
            this.userManager.Delete(adminUser);
            Assert.Null(serv.GetByUserName(USERNAME));
        }

        //Deletes
        [Test]
        public void testDeleteAdministratorSuccess()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            var result = this.userManager.Create(adminUser, "testpassword");

            if (result.Succeeded)
            {
                this.userManager.AddToRole(adminUser.Id, GlobalConstants.AdministratorRoleName);
            }
            serv.Add(entity);
            int count = serv.All().Count();

            Guid id = serv.GetByUserName(USERNAME).Id;

            // Act
            MichtavaResult res = serv.Delete(entity);
            var res2 = this.userManager.Delete(adminUser);

            // Assert
            Assert.True(res2.Succeeded);
            Assert.True(res is MichtavaSuccess);
            Assert.Null(serv.GetByUserName(USERNAME));
            Assert.True(serv.All().Count() == count - 1);
            Assert.True(serv.GetById(id).IsDeleted);

            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
            Assert.Null(serv.GetByUserName(USERNAME));

        }

       


        [Test]
        public void testDeleteAdministratorIsApplicationUserUpdated()
        {
            this.oneTimeSetUp();

            Assert.Null(serv.GetByUserName(USERNAME));

            // Arrange
            int count = serv.All().Count();
            Administrator c = serv.All().FirstOrDefault();
            string id = c.ApplicationUser.Id;
       

            // Act
            MichtavaResult res = serv.Delete(c);
            this.userManager.Delete(c.ApplicationUser);
            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.True(serv.All().Count() == count - 1);
            Assert.Null(ctx.Set<ApplicationUser>().Where(x => x.Id == id).FirstOrDefault());
      
            this.oneTimeSetUp();
        }

        private bool containsId(ICollection<HasId> col, Guid hId)
        {
            foreach (HasId h in col)
            {
                if (h.Id == hId)
                    return true;
            }
            return false;

        }

        [Test]
        public void testDeleteNonExistantAdministrator()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();

            entity.setId(Guid.NewGuid());
            Assert.Null(serv.GetById(entity.Id));

            // Act
            MichtavaResult res = serv.Delete(entity);


            // Assert
            Assert.True(res is MichtavaFailure);

        }



        [Test]
        public void testHardDeleteSuccessAdministrator()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            var result = this.userManager.Create(adminUser, "testpassword");

            if (result.Succeeded)
            {
                this.userManager.AddToRole(adminUser.Id, GlobalConstants.AdministratorRoleName);
            }
            MichtavaResult res2 = serv.Add(entity);
            int count = serv.All().Count();

            Guid id = serv.GetByUserName(USERNAME).Id;

            // Act
            MichtavaResult res = serv.HardDelete(entity);
            this.userManager.Delete(adminUser);
            Assert.Null(serv.GetByUserName(USERNAME));

            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.Null(serv.GetByUserName(USERNAME));
            Assert.True(serv.All().Count() == count - 1);
        }

        //Customs


       

      



    }
}
