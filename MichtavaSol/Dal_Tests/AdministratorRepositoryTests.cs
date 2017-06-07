
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
    class AdministratorRepositoryTests
    {
        private ApplicationDbContext ctx;

        private AdministratorRepository repo;

        private Administrator entity;

        private const string fn = "בדיקה";
        private const string ln = "בדיקה";

        [OneTimeSetUp]
        public void oneTimeSetUp()
        {
            var connection = DbConnectionFactory.CreateTransient();
            this.ctx = new ApplicationDbContext(connection);
            this.repo = new AdministratorRepository(ctx);

            new DatabaseSeeder().CreateDependenciesAndSeed(ctx);//heavy duty

        }

        [OneTimeTearDown]
        public void oneTimeTearDown()
        {
        }


        [SetUp]
        public void setUp()
        {
            entity = new Administrator();
            entity.FirstName = fn;
            entity.LastName = ln;
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

            Assert.True(repo.Count() == count+1);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();

        }

        [Test]
        public void testGetById()
        {
            // Arrange
            int count = repo.All().Count();

            Administrator c = repo.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Administrator actual = repo.GetById(c.Id);
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
            Administrator actual = repo.Get(x => (x.FirstName == fn &&
                                                x.LastName == ln)).FirstOrDefault();

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

            Assert.IsEmpty(repo.Get(x => x.FirstName == fn + "alala" &&
                                      x.LastName == ln));


            Assert.AreEqual(count + 1, repo.All().Count());
            entity.FirstName += "alala";


            // Act
            repo.Update(entity);
            repo.SaveChanges();

            // Assert

            Assert.NotNull(repo.Get(x => x.FirstName == fn + "alala" &&
                                       x.LastName == ln));

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

            Guid id = repo.Get(x => x.FirstName == fn &&
                                     x.LastName == ln).FirstOrDefault().Id;
            // Act
            repo.Delete(entity);
            repo.SaveChanges();
            // Assert

            Assert.IsEmpty(repo.Get(x => (x.FirstName == fn &&
                                                x.LastName == ln && x.IsDeleted == false)));

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
            Administrator actual = repo.Get(x => x.FirstName == fn &&
                                                x.LastName == ln).FirstOrDefault();

            // Assert

            Assert.NotNull(actual);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();
        }
    }

}
