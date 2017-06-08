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
    class TeacherServiceTests
    {
        private ApplicationDbContext ctx;

        private TeacherService serv;

        private Teacher entity;

        private ApplicationUser teacherUser;

        private UserManager<ApplicationUser> userManager;

        private RoleManager<IdentityRole> roleManager;

        private const string USERNAME = "testerTestiestu";

        private const string NAME = "טסטמוטי";


        [OneTimeSetUp]
        public void oneTimeSetUp()
        {
            var connection = DbConnectionFactory.CreateTransient();
            this.ctx = new ApplicationDbContext(connection);
            this.serv = new TeacherService(new TeacherRepository(ctx),
                                               new ApplicationUserRepository(ctx));
            var seeder = new DatabaseSeeder();
            seeder.CreateDependenciesAndSeed(ctx);//heavy duty
            this.userManager = seeder.userManager;
            this.roleManager = seeder.roleManager;

        }

        [SetUp]
        public void setUp()
        {
            teacherUser = new ApplicationUser()
            {
                UserName = USERNAME,
                Email = "something@somewhere.com"
            };


            entity = new Teacher(teacherUser, NAME);
        }

        //Adds

        //Test Case ID: TS1
        [Test]
        public void testAddStandaloneTeacher()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();


            // Act
            var result = this.userManager.Create(teacherUser, "testpassword");

            if (result.Succeeded)
            {
                this.userManager.AddToRole(teacherUser.Id, GlobalConstants.TeacherRoleName);
            }
            MichtavaResult res = serv.Add(entity);

            // Assert
            Assert.AreEqual(count + 1, serv.All().Count());
            Assert.NotNull(serv.GetByUserName(USERNAME));
            Assert.True(res is MichtavaSuccess);
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
            this.userManager.Delete(teacherUser);
            Assert.Null(serv.GetByUserName(USERNAME));
        }

        //Test Case ID: TS2
        [Test]
        public void testAddTeacherExistingId()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();
            Teacher existing = serv.All().FirstOrDefault();

            // Act

            Assert.Throws<DbEntityValidationException>(() => this.userManager.Create(existing.ApplicationUser, "testpassword"));
            oneTimeSetUp();

        }

        //Test Case ID: TS3
        [Test]
        public void testAddExistingTeacherDetails()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();
            Teacher existing = serv.All().FirstOrDefault();
            Teacher c = new Teacher(existing.ApplicationUser, existing.Name);

            // Act
            Assert.Throws<DbEntityValidationException>(() => this.userManager.Create(existing.ApplicationUser, "testpassword"));
            oneTimeSetUp();


        }

        //Gets

        //Test Case ID: TS4
        [Test]
        public void testGetTeacherByIdTrue()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();

            Teacher c = serv.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Teacher actual = serv.GetById(c.Id);
            // Assert

            Assert.NotNull(actual);

        }


        //Test Case ID: TS5
        [Test]
        public void testGetTeacherByIdFalse()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();

            // Act
            Teacher actual = serv.GetById(Guid.NewGuid());
            // Assert

            Assert.Null(actual);

        }


        //Test Case ID: TS6
        [Test]
        public void testGetTeacherByDetailsTrue()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();

            Teacher c = serv.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Teacher actual = serv.GetByUserName(c.ApplicationUser.UserName);

            // Assert
            Assert.NotNull(actual);
        }



        //Test Case ID: TS7
        [Test]
        public void testGetTeacherByDetailsFalse()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();


            // Act
            Teacher actual = serv.GetByUserName("fsajklasjfkslajk");

            // Assert

            Assert.Null(actual);
        }

        //Update

        //Test Case ID: TS8
        [Test]
        public void testUpdateTeacherSuccess()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();

            var result = this.userManager.Create(teacherUser, "testpassword");

            if (result.Succeeded)
            {
                this.userManager.AddToRole(teacherUser.Id, GlobalConstants.TeacherRoleName);
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
            this.userManager.Delete(teacherUser);
            Assert.Null(serv.GetByUserName(USERNAME));
        }


        //Test Case ID: TS9
        [Test]
        public void testUpdateTeacherNonExistant()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();

            // Act
            MichtavaResult res = serv.Update(new Teacher(null, "דג"));

            // Assert
            Assert.True(res is MichtavaFailure);
        }

        //Test Case ID: TS10
        [Test]
        public void testAddNullAppUser()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();
            Teacher existing = serv.All().FirstOrDefault();

            // Act
            Assert.Throws<DbEntityValidationException>(() => this.userManager.Create(existing.ApplicationUser, "testpassword"));
            oneTimeSetUp();
        }

        //Test Case ID: TS11
        [Test]
        public void testUpdateTeacherExistingDetails()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();
            Teacher c = serv.All().FirstOrDefault();
            var result = this.userManager.Create(teacherUser, "testpassword");

            if (result.Succeeded)
            {
                this.userManager.AddToRole(teacherUser.Id, GlobalConstants.TeacherRoleName);
            }
            serv.Add(entity);

            Teacher study = serv.GetByUserName(USERNAME);
            Guid id = study.Id;

            Assert.AreEqual(count + 1, serv.All().Count());

            //Act
            entity.ApplicationUser.UserName = c.ApplicationUser.UserName;
            var res = serv.Update(entity);


            // Assert
            Assert.True(res is MichtavaFailure);
            entity.ApplicationUser.UserName = USERNAME;



            Assert.True(serv.GetById(id).ApplicationUser.UserName == USERNAME);
            Assert.True(serv.HardDelete(serv.GetById(id)) is MichtavaSuccess);
            this.userManager.Delete(teacherUser);
            Assert.Null(serv.GetByUserName(USERNAME));
        }

        //Deletes

        //Test Case ID: TS12
        [Test]
        public void testDeleteTeacherSuccess()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            var result = this.userManager.Create(teacherUser, "testpassword");

            if (result.Succeeded)
            {
                this.userManager.AddToRole(teacherUser.Id, GlobalConstants.TeacherRoleName);
            }
            serv.Add(entity);
            int count = serv.All().Count();

            Teacher study = serv.GetByUserName(USERNAME);
            Guid id = study.Id;

            // Act
            MichtavaResult res = serv.Delete(entity);
            var res2 = this.userManager.Delete(teacherUser);

            // Assert
            Assert.True(res2.Succeeded);
            Assert.True(res is MichtavaSuccess);
            Assert.Null(serv.GetByUserName(USERNAME));
            Assert.True(serv.All().Count() == count - 1);
            Assert.True(serv.GetById(id).IsDeleted);

            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
            Assert.Null(serv.GetByUserName(USERNAME));

        }




        //Test Case ID: TS13
        [Test]
        public void testDeleteTeacherIsApplicationUserUpdated()
        {
            this.oneTimeSetUp();

            Assert.Null(serv.GetByUserName(USERNAME));

            // Arrange
            int count = serv.All().Count();
            Teacher c = serv.All().FirstOrDefault();
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

        //Test Case ID: TS14
        [Test]
        public void testDeleteNonExistantTeacher()
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



        //Test Case ID: TS15
        [Test]
        public void testHardDeleteSuccessTeacher()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            var result = this.userManager.Create(teacherUser, "testpassword");

            if (result.Succeeded)
            {
                this.userManager.AddToRole(teacherUser.Id, GlobalConstants.TeacherRoleName);
            }
            MichtavaResult res2 = serv.Add(entity);
            int count = serv.All().Count();

            Teacher study = serv.GetByUserName(USERNAME);
            Guid id = study.Id;

            // Act
            MichtavaResult res = serv.HardDelete(entity);
            this.userManager.Delete(teacherUser);
            Assert.Null(serv.GetByUserName(USERNAME));

            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.Null(serv.GetByUserName(USERNAME));
            Assert.True(serv.All().Count() == count - 1);
        }

        //Customs








    }
}
