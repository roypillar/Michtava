using Dal;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Entities.Models;
using Dal.Repositories;
using System.Data.Common;
using Effort;
using Common;

namespace Dal_Tests
{
    class SchoolClassRepositoryTests
    { 
        private IApplicationDbContext ctx;

        private SchoolClassRepository repo;

        private SchoolClass entity;

        [OneTimeSetUp]
        public void oneTimeSetUp()
        {
            var connection = DbConnectionFactory.CreateTransient();
            this.ctx = new ApplicationDbContext(connection);
            this.repo = new SchoolClassRepository(ctx);

        }

        [OneTimeTearDown]
        public void oneTimeTearDown()
        {
            //nothing for now
        }


        [SetUp]
        public void setUp()
        {
            entity = new SchoolClass();
            entity.setId(Guid.NewGuid());//consider using
        }

        [TearDown]
        public void tearDown()
        {

        }


        [Test]
        public void testAdd()
        {
            // Arrange
            int count = repo.All().Count();


            // Act
            repo.Add(entity);
            repo.SaveChanges();

            // Assert
            Assert.AreEqual(count + 1, repo.All().Count());


            //TODO add existing test in Subjects
        }

        [Test]
        public void testGetById()
        {
            // Arrange
            int count = repo.All().Count();
            repo.Add(entity);
            repo.SaveChanges();
            Assert.AreEqual(count + 1, repo.All().Count());


            // Act
            SchoolClass actual = repo.GetById(entity.Id);
            // Assert
            Assert.NotNull(actual);

            //TODO add existing test
        }

        [Test]
        public void testGet()
        {
            // Arrange
            int count = repo.All().Count();
            repo.Add(entity);
            repo.SaveChanges();
            Assert.AreEqual(count + 1, repo.All().Count());


            // Act
            SchoolClass actual = repo.GetById(entity.Id);
            // Assert
            Assert.NotNull(actual);

            //TODO add existing test
        }


        [Test]
        public void testUpdate()
        {
            // Arrange
            int count = repo.All().Count();
            repo.Add(entity);
            Assert.AreEqual(count + 1, repo.All().Count());

            int expected = entity.TestID + GlobalConstants.numOfTests;
            entity.TestID += GlobalConstants.numOfTests;

            // Act
            repo.Update(entity);
            // Assert
            Assert.NotNull(repo.GetByTestID(expected));//this is fine, because of course the DB is empty beforehand.

            //TODO add existing test
        }

        [Test]
        public void testDelete()
        {
            // Arrange
            int count = repo.All().Count();
            repo.Add(entity);
            Assert.AreEqual(count + 1, repo.All().Count());

            // Act
            repo.Delete(entity);
            // Assert

            Assert.IsTrue(repo.GetByTestID(entity.TestID).IsDeleted);//this is fine, because of course the DB is empty beforehand.

            //TODO add all remaining methods
        }

        [Test]
        public void testGetByDetails()
        {
            // Arrange
            int count = repo.All().Count();
            entity.ClassLetter = "ט";
            entity.ClassNumber = 7;

            repo.Add(entity);
            Assert.AreEqual(count + 1, repo.All().Count());


            // Act
            // Assert
            Assert.NotNull(repo.GetByTestID(entity.TestID));//this is fine, because of course the DB is empty beforehand.
            Assert.NotNull(repo.GetByDetails(7, "ט"));

        }
    }

}
