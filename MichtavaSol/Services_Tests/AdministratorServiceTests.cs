//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNet.Identity.EntityFramework;
//using System.Data.Entity;
//using Entities.Models;
//using System.Data.Common;
//using Effort;
//using Common;
//using Dal;
//using Services;
//using Dal.Repositories;
//using NUnit.Framework;

//namespace Services_Tests
//{
//    class AdministratorServiceTests
//    {
//        private ApplicationDbContext ctx;

//        private AdministratorService serv;

//        private Administrator entity;

//        private const string hwTitl = "טסטינגטון";

//        private const string hwDesc = "555טסטינגטון";

//        [OneTimeSetUp]
//        public void oneTimeSetUp()
//        {
//            var connection = DbConnectionFactory.CreateTransient();
//            this.ctx = new ApplicationDbContext(connection);
//            this.serv = new AdministratorService(new AdministratorRepository(ctx),
//                                                 new ApplicationUserRepository(ctx));
//            new DatabaseSeeder().CreateDependenciesAndSeed(ctx);//heavy duty

//        }

//        [SetUp]
//        public void setUp()
//        {
//            entity = new Administrator(,);
//        }

//        //Adds
//        [Test]
//        public void testAddStandaloneAdministrator()
//        {
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            // Arrange
//            int count = serv.All().Count();


//            // Act
//            MichtavaResult res = serv.Add(entity);

//            // Assert
//            Assert.AreEqual(count + 1, serv.All().Count());
//            Assert.NotNull(serv.GetByDetails(hwTitl, hwDesc));
//            Assert.True(res is MichtavaSuccess);
//            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
//        }

//        [Test]
//        public void testAddExistingIdAdministrator()
//        {
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            // Arrange
//            int count = serv.All().Count();
//            Administrator existing = serv.All().FirstOrDefault();

//            // Act
//            MichtavaResult res = serv.Add(existing);

//            // Assert
//            Assert.True(res is MichtavaFailure);
//        }

//        [Test]
//        public void testAddExistingDetailsAdministrator()
//        {
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            // Arrange
//            int count = serv.All().Count();
//            Administrator existing = serv.All().FirstOrDefault();

//            Administrator c = new Administrator(existing.Title, existing.Description, DateTime.Now,
//                 ctx.Set<Teacher>().FirstOrDefault(),
//                 ctx.Set<Text>().FirstOrDefault());
//            // Act

//            MichtavaResult res = serv.Add(c);

//            // Assert
//            Assert.True(res is MichtavaFailure);

//        }

//        [Test]
//        public void testAddNullTextAdministrator()
//        {
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            // Arrange
//            int count = serv.All().Count();
//            Administrator existing = serv.All().FirstOrDefault();

//            Administrator c = new Administrator();
//            c.Title = hwTitl;
//            c.Description = hwDesc;
//            c.Deadline = DateTime.Now;
//            c.Created_By = ctx.Set<Teacher>().FirstOrDefault();

//            // Act
//            MichtavaResult res = serv.Add(c);

//            // Assert
//            Assert.True(res is MichtavaFailure);

//        }

//        [Test]
//        public void testAddNullTeacherAdministrator()
//        {
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            // Arrange
//            int count = serv.All().Count();
//            Administrator existing = serv.All().FirstOrDefault();

//            Administrator c = new Administrator();
//            c.Title = hwTitl;
//            c.Description = hwDesc;
//            c.Deadline = DateTime.Now;
//            c.Text = ctx.Set<Text>().FirstOrDefault();

//            // Act
//            MichtavaResult res = serv.Add(c);

//            // Assert
//            Assert.True(res is MichtavaFailure);

//        }

//        [Test]
//        public void testAddNullDateAdministrator()
//        {
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            // Arrange
//            int count = serv.All().Count();
//            Administrator existing = serv.All().FirstOrDefault();

//            Administrator c = new Administrator();
//            c.Title = hwTitl;
//            c.Description = hwDesc;
//            c.Text = ctx.Set<Text>().FirstOrDefault();
//            c.Created_By = ctx.Set<Teacher>().FirstOrDefault();
//            //c.Teacher_Id = c.Created_By.Id;
//            //c.Text_Id = c.Text.Id;
//            // Act
//            MichtavaResult res = serv.Add(c);

//            // Assert
//            Assert.True(res is MichtavaSuccess);
//            Assert.NotNull(serv.GetByDetails(hwTitl, hwDesc).Deadline);
//            Assert.True(serv.HardDelete(c) is MichtavaSuccess);
//        }

//        [Test]
//        public void testAddWithTextInconsistencyAdministrator()
//        {
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//        }

//        //Gets
//        [Test]
//        public void testGetAdministratorByIdTrue()
//        {
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            // Arrange
//            int count = serv.All().Count();

//            Administrator c = serv.All().FirstOrDefault();
//            Assert.NotNull(c);


//            // Act
//            Administrator actual = serv.GetById(c.Id);
//            // Assert

//            Assert.NotNull(actual);

//        }

//        [Test]
//        public void testGetAdministratorByIdFalse()
//        {
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            // Arrange
//            int count = serv.All().Count();

//            // Act
//            Administrator actual = serv.GetById(Guid.NewGuid());
//            // Assert

//            Assert.Null(actual);

//        }


//        [Test]
//        public void testGetAdministratorByDetailsTrue()
//        {
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            // Arrange
//            int count = serv.All().Count();

//            Administrator c = serv.All().FirstOrDefault();
//            Assert.NotNull(c);


//            // Act
//            Administrator actual = serv.GetByDetails(c.Title, c.Description);
//            // Assert

//            Assert.NotNull(actual);
//        }



//        [Test]
//        public void testGetAdministratorByDetailsFalse()
//        {
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            // Arrange
//            int count = serv.All().Count();


//            // Act
//            Administrator actual = serv.GetByDetails("14", "אסטערכן");
//            // Assert

//            Assert.Null(actual);

//        }

//        //Update
//        [Test]
//        public void testUpdateAdministratorSuccess()
//        {
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            // Arrange
//            int count = serv.All().Count();
//            serv.Add(entity);

//            Assert.Null(serv.GetByDetails("98", hwDesc));


//            Assert.AreEqual(count + 1, serv.All().Count());
//            entity.Title = "98";


//            // Act
//            MichtavaResult res = serv.Update(entity);

//            // Assert
//            Assert.True(res is MichtavaSuccess);
//            Assert.NotNull(serv.GetByDetails("98", hwDesc));
//            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
//        }

//        [Test]
//        public void testUpdateAdministratorNonExistant()
//        {
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            // Arrange
//            int count = serv.All().Count();

//            Assert.Null(serv.GetByDetails("98", hwDesc));


//            entity.Title = "98";


//            // Act
//            MichtavaResult res = serv.Update(new Administrator("98", hwDesc, DateTime.Now, ctx.Set<Teacher>().FirstOrDefault(), ctx.Set<Text>().FirstOrDefault()));

//            // Assert
//            Assert.True(res is MichtavaFailure);
//        }

//        [Test]
//        public void testUpdateAdministratorExistingDetails()
//        {
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            // Arrange
//            int count = serv.All().Count();
//            Administrator c = serv.All().FirstOrDefault();

//            serv.Add(entity);

//            Assert.AreEqual(count + 1, serv.All().Count());
//            entity.Title = c.Title;
//            entity.Description = c.Description;


//            // Act
//            MichtavaResult res = serv.Update(entity);

//            // Assert
//            Assert.True(res is MichtavaFailure);
//            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
//        }

//        [Test]
//        public void testUpdateAdministratorNullText()
//        {
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            // Arrange
//            int count = serv.All().Count();
//            Administrator c = serv.All().FirstOrDefault();

//            serv.Add(entity);

//            Assert.AreEqual(count + 1, serv.All().Count());

//            entity.Text = null;

//            // Act
//            MichtavaResult res = serv.Update(entity);

//            // Assert
//            Assert.True(res is MichtavaFailure);
//            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
//        }

//        [Test]
//        public void testUpdateAdministratorNullTeacher()
//        {
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            // Arrange
//            int count = serv.All().Count();
//            Administrator c = serv.All().FirstOrDefault();

//            serv.Add(entity);

//            Assert.AreEqual(count + 1, serv.All().Count());

//            entity.Created_By = null;

//            // Act
//            MichtavaResult res = serv.Update(entity);

//            // Assert
//            Assert.True(res is MichtavaFailure);
//            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
//        }

//        //Deletes
//        [Test]
//        public void testDeleteAdministratorSuccess()
//        {
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            // Arrange
//            serv.Add(entity);
//            int count = serv.All().Count();

//            Guid id = serv.GetByDetails(hwTitl, hwDesc).Id;

//            // Act
//            MichtavaResult res = serv.Delete(entity);


//            // Assert
//            Assert.True(res is MichtavaSuccess);
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            Assert.True(serv.All().Count() == count - 1);
//            Assert.True(serv.GetById(id).IsDeleted);

//            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);


//        }



//        private bool containsId(ICollection<HasId> col, Guid hId)
//        {
//            foreach (HasId h in col)
//            {
//                if (h.Id == hId)
//                    return true;
//            }
//            return false;

//        }

//        [Test]
//        public void testDeleteNonExistant()
//        {
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            // Arrange
//            int count = serv.All().Count();

//            entity.setId(Guid.NewGuid());
//            Assert.Null(serv.GetById(entity.Id));

//            // Act
//            MichtavaResult res = serv.Delete(entity);


//            // Assert
//            Assert.True(res is MichtavaFailure);

//        }



//        [Test]
//        public void testHardDeleteSuccessAdministrator()
//        {
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            // Arrange
//            serv.Add(entity);
//            int count = serv.All().Count();

//            Guid id = serv.GetByDetails(hwTitl, hwDesc).Id;

//            // Act
//            MichtavaResult res = serv.HardDelete(entity);


//            // Assert
//            Assert.True(res is MichtavaSuccess);
//            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
//            Assert.True(serv.All().Count() == count - 1);
//        }

//        //Customs





//        //add question, remove question, quickly tho

//    }
//}
