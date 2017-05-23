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

namespace Services_Tests
{
    class SubjectServiceTests
    {
        private ApplicationDbContext ctx;

        private SubjectService serv;

        private Subject entity;

        private const string n = "טסטינגטון";


        [OneTimeSetUp]
        public void oneTimeSetUp()
        {
            var connection = DbConnectionFactory.CreateTransient();
            this.ctx = new ApplicationDbContext(connection);
            this.serv = new SubjectService(new SubjectRepository(ctx));
            new DatabaseSeeder().CreateDependenciesAndSeed(ctx);//heavy duty

        }

        [SetUp]
        public void setUp()
        {
            entity = new Subject(n);
        }

        //Adds
        [Test]
        public void testAddStandalone()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();


            // Act
            MichtavaResult res = serv.Add(entity);

            // Assert
            Assert.AreEqual(count + 1, serv.All().Count());
            Assert.NotNull(serv.GetByName(n));
            Assert.True(res is MichtavaSuccess);
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }

        [Test]
        public void testAddExistingId()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();
            Subject existing = serv.All().FirstOrDefault();

            // Act
            MichtavaResult res = serv.Add(existing);

            // Assert
            Assert.True(res is MichtavaFailure);
        }

        [Test]
        public void testAddExistingDetails()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();
            Subject existing = serv.All().FirstOrDefault();

            Subject c = new Subject(existing.Name);
            // Act

            MichtavaResult res = serv.Add(c);

            // Assert
            Assert.True(res is MichtavaFailure);

        }

      
        [Test]
        public void testAddWithTextInconsistency()
        {
            Assert.Null(serv.GetByName(n));
        }


        [Test]
        public void testAddWithTeacherInconsistency()
        {
            Assert.Null(serv.GetByName(n));
        }

        //Gets
        [Test]
        public void testGetByIdTrue()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();

            Subject c = serv.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Subject actual = serv.GetById(c.Id);
            // Assert

            Assert.NotNull(actual);

        }

        [Test]
        public void testGetByIdFalse()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();

            // Act
            Subject actual = serv.GetById(Guid.NewGuid());
            // Assert

            Assert.Null(actual);

        }


        [Test]
        public void testGetByDetailsTrue()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();

            Subject c = serv.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Subject actual = serv.GetByName(c.Name);
            // Assert

            Assert.NotNull(actual);
        }



        [Test]
        public void testGetByDetailsFalse()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();


            // Act
            Subject actual = serv.GetByName("14");
            // Assert

            Assert.Null(actual);

        }

        //Update
        [Test]
        public void testUpdateSuccess()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();
            serv.Add(entity);

            Assert.Null(serv.GetByName("harambe"));


            Assert.AreEqual(count + 1, serv.All().Count());
            entity.Name = "harambe";


            // Act
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.NotNull(serv.GetByName("harambe"));
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }

        [Test]
        public void testUpdateNonExistant()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();

            Assert.Null(serv.GetByName("harambe"));


            entity.Name = "harambe";


            // Act
            MichtavaResult res = serv.Update(new Subject("harambe"));
            // Assert
            Assert.True(res is MichtavaFailure);
        }

        [Test]
        public void testUpdateExistingDetails()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();
            Subject c = serv.All().FirstOrDefault();

            serv.Add(entity);

            Assert.AreEqual(count + 1, serv.All().Count());
            entity.Name = c.Name;


            // Act
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaFailure);
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }

       

        //Deletes
        [Test]
        public void testDeleteSuccess()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            serv.Add(entity);
            int count = serv.All().Count();

            Guid id = serv.GetByName(n).Id;

            // Act
            MichtavaResult res = serv.Delete(entity);


            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.Null(serv.GetByName(n));
            Assert.True(serv.All().Count() == count - 1);
            Assert.True(serv.GetById(id).IsDeleted);

            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);


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
        public void testDeleteNonExistant()
        {
            Assert.Null(serv.GetByName(n));
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
        public void testHardDeleteSuccess()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            serv.Add(entity);
            int count = serv.All().Count();

            Guid id = serv.GetByName(n).Id;

            // Act
            MichtavaResult res = serv.HardDelete(entity);


            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.Null(serv.GetByName(n));
            Assert.True(serv.All().Count() == count - 1);
        }

        //Customs





        //add question, remove question, quickly tho

    }
}
