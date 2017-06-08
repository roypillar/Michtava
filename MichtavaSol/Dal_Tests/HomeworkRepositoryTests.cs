
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
    class HomeworkRepositoryTests
    {
        private ApplicationDbContext ctx;

        private HomeworkRepository repo;

        private Homework entity;

        private const string hwTitle = "בדיקה בדיקה";
        private const string hwDesc = "בדיקה";

        [OneTimeSetUp]
        public void oneTimeSetUp()
        {
            var connection = DbConnectionFactory.CreateTransient();
            this.ctx = new ApplicationDbContext(connection);
            this.repo = new HomeworkRepository(ctx);
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
            entity = new Homework(hwTitle,hwDesc, DateTime.Now,
                  ctx.Set<Teacher>().FirstOrDefault(),
                  ctx.Set<Text>().FirstOrDefault());
            //entity.setId(Guid.NewGuid());//consider using
        }

        [TearDown]
        public void tearDown()
        {
        }


        //Test Case ID: HWR1
        [Test]
        public void testAdd()
        {
            //Arrange
            int count = repo.All().Count();


            // Act
            this.repo.Add(entity);
            this.repo.SaveChanges();
            // Assert

            Assert.NotNull(repo.GetByDetails(hwTitle,hwDesc));

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();

        }

        //Test Case ID: HWR2
        [Test]
        public void testGetById()
        {
            // Arrange
            int count = repo.All().Count();

            Homework c = repo.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Homework actual = repo.GetById(c.Id);
            // Assert

            Assert.NotNull(actual);

        }

        //Test Case ID: HWR3
        [Test]
        public void testGet()
        {
            // Arrange
            int count = repo.All().Count();
            this.repo.Add(entity);
            this.repo.SaveChanges();


            // Act
            Homework actual = repo.Get(x => (x.Title == hwTitle &&
                                                x.Description == hwDesc)).FirstOrDefault();

            // Assert

            Assert.NotNull(actual);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();

        }


        //Test Case ID: HWR4
        [Test]
        public void testUpdate()
        {

            // Arrange
            int count = repo.All().Count();
            repo.Add(entity);
            this.repo.SaveChanges();

            Assert.Null(repo.GetByDetails(hwTitle+"alala",hwDesc));


            Assert.AreEqual(count + 1, repo.All().Count());
            entity.Title += "alala";


            // Act
            repo.Update(entity);
            repo.SaveChanges();

            // Assert

            Assert.NotNull(repo.GetByDetails(hwTitle+"alala",hwDesc));
            this.repo.HardDelete(entity);
            this.repo.SaveChanges();

        }

        //Test Case ID: HWR5
        [Test]
        public void testDelete()
        {
            // Arrange
            repo.Add(entity);
            repo.SaveChanges();
            int count = repo.All().Count();

            Guid id = repo.GetByDetails(hwTitle,hwDesc).Id;
            // Act
            repo.Delete(entity);
            repo.SaveChanges();
            // Assert

            Assert.Null(repo.GetByDetails(hwTitle,hwDesc));
            Assert.True(repo.All().Count() == count - 1);
            Assert.True(repo.GetById(id).IsDeleted);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();

            //TODO add all remaining methods
        }

        //Test Case ID: HWR6
        [Test]
        public void testGetByDetails()
        {
            // Arrange
            int count = repo.All().Count();
            this.repo.Add(entity);
            this.repo.SaveChanges();


            // Act
            Homework actual = repo.GetByDetails(hwTitle,hwDesc);

            // Assert

            Assert.NotNull(actual);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();
        }
    }

}
