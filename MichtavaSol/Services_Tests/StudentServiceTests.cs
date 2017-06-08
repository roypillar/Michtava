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
    class StudentServiceTests
    {
        private ApplicationDbContext ctx;

        private StudentService serv;

        private Student entity;

        private ApplicationUser studentUser;

        private UserManager<ApplicationUser> userManager;

        private RoleManager<IdentityRole> roleManager;

        private const string USERNAME = "testerTestiestu";

        private const string NAME = "טסטמוטי";


        [OneTimeSetUp]
        public void oneTimeSetUp()
        {
            var connection = DbConnectionFactory.CreateTransient();
            this.ctx = new ApplicationDbContext(connection);
            this.serv = new StudentService(new StudentRepository(ctx),
                                               new ApplicationUserRepository(ctx));
            var seeder = new DatabaseSeeder();
            seeder.CreateDependenciesAndSeed(ctx);//heavy duty
            this.userManager = seeder.userManager;
            this.roleManager = seeder.roleManager;

        }

        [SetUp]
        public void setUp()
        {
            studentUser = new ApplicationUser()
            {
                UserName = USERNAME,
                Email = "something@somewhere.com"
            };


            entity = new Student(studentUser, NAME);
        }

        //Adds

        //Test Case ID: SS1
        [Test]
        public  void testAddStandaloneStudent()
        {
             Assert.Null( serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();


            // Act
            var result = this.userManager.Create(studentUser, "testpassword");

            if (result.Succeeded)
            {
                this.userManager.AddToRole(studentUser.Id, GlobalConstants.StudentRoleName);
            }
            MichtavaResult res = serv.Add(entity);

            // Assert
            Assert.AreEqual(count + 1, serv.All().Count());
            Assert.NotNull(serv.GetByUserName(USERNAME));
            Assert.True(res is MichtavaSuccess);
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
            this.userManager.Delete(studentUser);
            Assert.Null(serv.GetByUserName(USERNAME));
        }


        //Test Case ID: SS2
        [Test]
        public void testAddStudentExistingId()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();
            Student existing = serv.All().FirstOrDefault();

            // Act

            Assert.Throws<DbEntityValidationException>(() => this.userManager.Create(existing.ApplicationUser, "testpassword"));
            oneTimeSetUp();

        }


        //Test Case ID: SS3
        [Test]
        public void testAddExistingStudentDetails()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();
            Student existing = serv.All().FirstOrDefault();
            Student c = new Student(existing.ApplicationUser, existing.Name);

            // Act
            Assert.Throws<DbEntityValidationException>(() => this.userManager.Create(existing.ApplicationUser, "testpassword"));
            oneTimeSetUp();


        }

        //Gets

        //Test Case ID: SS4
        [Test]
        public void testGetStudentByIdTrue()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();

            Student c = serv.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Student actual = serv.GetById(c.Id);
            // Assert

            Assert.NotNull(actual);

        }


        //Test Case ID: SS5
        [Test]
        public void testGetStudentByIdFalse()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();

            // Act
            Student actual = serv.GetById(Guid.NewGuid());
            // Assert

            Assert.Null(actual);

        }


        //Test Case ID: SS6
        [Test]
        public  void testGetStudentByDetailsTrue()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();

            Student c = serv.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Student actual =  serv.GetByUserName(c.ApplicationUser.UserName);

            // Assert
            Assert.NotNull(actual);
        }



        //Test Case ID: SS7
        [Test]
        public  void testGetStudentByDetailsFalse()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();


            // Act
            Student actual =  serv.GetByUserName("fsajklasjfkslajk");

            // Assert

            Assert.Null(actual);
        }

        //Update

        //Test Case ID: SS8
        [Test]
        public void testUpdateStudentSuccess()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();

            var result = this.userManager.Create(studentUser, "testpassword");

            if (result.Succeeded)
            {
                this.userManager.AddToRole(studentUser.Id, GlobalConstants.StudentRoleName);
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
            this.userManager.Delete(studentUser);
            Assert.Null(serv.GetByUserName(USERNAME));
        }

        //Test Case ID: SS9
        [Test]
        public void testUpdateStudentNonExistant()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();

            // Act
            MichtavaResult res = serv.Update(new Student(null, "דג"));

            // Assert
            Assert.True(res is MichtavaFailure);
        }


        //Test Case ID: SS10
        [Test]
        public void testAddNullAppUser()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();
            Student existing = serv.All().FirstOrDefault();

            // Act
            Assert.Throws<DbEntityValidationException>(() => this.userManager.Create(existing.ApplicationUser, "testpassword"));
            oneTimeSetUp();
        }

        //Test Case ID: SS11
        [Test]
        public  void testUpdateStudentExistingDetails()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            int count = serv.All().Count();
            Student c = serv.All().FirstOrDefault();
            var result = this.userManager.Create(studentUser, "testpassword");

            if (result.Succeeded)
            {
                this.userManager.AddToRole(studentUser.Id, GlobalConstants.StudentRoleName);
            }
            serv.Add(entity);

            Student study =  serv.GetByUserName(USERNAME);
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
            this.userManager.Delete(studentUser);
            Assert.Null(serv.GetByUserName(USERNAME));
        }

        //Deletes

        //Test Case ID: SS12
        [Test]
        public  void testDeleteStudentSuccess()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            var result = this.userManager.Create(studentUser, "testpassword");

            if (result.Succeeded)
            {
                this.userManager.AddToRole(studentUser.Id, GlobalConstants.StudentRoleName);
            }
            serv.Add(entity);
            int count = serv.All().Count();

            Student study =  serv.GetByUserName(USERNAME);
            Guid id = study.Id;

            // Act
            MichtavaResult res = serv.Delete(entity);
            var res2 = this.userManager.Delete(studentUser);

            // Assert
            Assert.True(res2.Succeeded);
            Assert.True(res is MichtavaSuccess);
            Assert.Null(serv.GetByUserName(USERNAME));
            Assert.True(serv.All().Count() == count - 1);
            Assert.True(serv.GetById(id).IsDeleted);

            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
            Assert.Null(serv.GetByUserName(USERNAME));

        }




        //Test Case ID: SS13
        [Test]
        public void testDeleteStudentIsApplicationUserUpdated()
        {
            this.oneTimeSetUp();

            Assert.Null(serv.GetByUserName(USERNAME));

            // Arrange
            int count = serv.All().Count();
            Student c = serv.All().FirstOrDefault();
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

        //Test Case ID: SS14
        [Test]
        public void testDeleteNonExistantStudent()
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



        //Test Case ID: SS15
        [Test]
        public  void testHardDeleteSuccessStudent()
        {
            Assert.Null(serv.GetByUserName(USERNAME));
            // Arrange
            var result = this.userManager.Create(studentUser, "testpassword");

            if (result.Succeeded)
            {
                this.userManager.AddToRole(studentUser.Id, GlobalConstants.StudentRoleName);
            }
            MichtavaResult res2 = serv.Add(entity);
            int count = serv.All().Count();

            Student study =  serv.GetByUserName(USERNAME);
            Guid id = study.Id;

            // Act
            MichtavaResult res = serv.HardDelete(entity);
            this.userManager.Delete(studentUser);
            Assert.Null(serv.GetByUserName(USERNAME));

            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.Null(serv.GetByUserName(USERNAME));
            Assert.True(serv.All().Count() == count - 1);
        }

        //Customs








    }
}
