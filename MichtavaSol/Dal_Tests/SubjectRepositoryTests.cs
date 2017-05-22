
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
    class SubjectRepositoryTests
    {
        private ApplicationDbContext ctx;

        private SubjectRepository repo;

        private Subject entity;

        private const string n = "בדיקה בדיקה";

        [OneTimeSetUp]
        public void oneTimeSetUp()
        {
            var connection = DbConnectionFactory.CreateTransient();
            this.ctx = new ApplicationDbContext(connection);
            this.repo = new SubjectRepository(ctx);
            new DatabaseSeeder().CreateDependenciesAndSeed(ctx);//heavy duty

        }

        [OneTimeTearDown]
        public void oneTimeTearDown()
        {
            //nothing for now
        }


        [SetUp]
        public void setUp()
        {
            entity = new Subject(n);
        }

        [TearDown]
        public void tearDown()
        {
        }

        [Test]
        public void testAdd()
        {
            //Arrange
            int count = repo.All().Count();


            // Act
            this.repo.Add(entity);
            this.repo.SaveChanges();
            // Assert

            Assert.NotNull(repo.GetByName(n));

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();

        }

        [Test]
        public void testGetById()
        {
            // Arrange
            int count = repo.All().Count();

            Subject c = repo.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Subject actual = repo.GetById(c.Id);
            // Assert

            Assert.NotNull(actual);

        }

        [Test]
        public void testGet()
        {
            // Arrange
            int count = repo.All().Count();
            this.repo.Add(entity);
            this.repo.SaveChanges();


            // Act
            Subject actual = repo.Get(x => (x.Name == n)).FirstOrDefault();

            // Assert

            Assert.NotNull(actual);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();

        }


        [Test]
        public void testUpdate()
        {

            // Arrange
            int count = repo.All().Count();
            repo.Add(entity);
            this.repo.SaveChanges();

            Assert.Null(repo.GetByName(n + "alala"));


            Assert.AreEqual(count + 1, repo.All().Count());
            entity.Name += "alala";


            // Act
            repo.Update(entity);
            repo.SaveChanges();

            // Assert

            Assert.NotNull(repo.GetByName(n + "alala"));
            this.repo.HardDelete(entity);
            this.repo.SaveChanges();

        }

        [Test]
        public void testDelete()
        {
            // Arrange
            repo.Add(entity);
            repo.SaveChanges();
            int count = repo.All().Count();

            Guid id = repo.GetByName(n).Id;
            // Act
            repo.Delete(entity);
            repo.SaveChanges();
            // Assert

            Assert.Null(repo.GetByName(n));
            Assert.True(repo.All().Count() == count - 1);
            Assert.True(repo.GetById(id).IsDeleted);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();

            //TODO add all remaining methods
        }

        [Test]
        public void testGetByDetails()
        {
            // Arrange
            int count = repo.All().Count();
            this.repo.Add(entity);
            this.repo.SaveChanges();


            // Act
            Subject actual = repo.GetByName(n);

            // Assert

            Assert.NotNull(actual);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();
        }


        [Test]
        public void testGetByName()
        {
            // Arrange
            int count = repo.All().Count();
            this.repo.Add(entity);
            this.repo.SaveChanges();


            // Act
            Subject actual = repo.GetByName(n);

            // Assert

            Assert.NotNull(actual);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();
        }


    }

}
