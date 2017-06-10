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
        private ApplicationDbContext ctx;

        private SchoolClassRepository repo;

        private SchoolClass entity;

        [OneTimeSetUp]
        public void oneTimeSetUp()
        {
            var connection = DbConnectionFactory.CreateTransient();
            this.ctx = new ApplicationDbContext(connection);
            this.repo = new SchoolClassRepository(ctx);
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
            entity = new SchoolClass(22,"ס");
            //entity.setId(Guid.NewGuid());//consider using
        }

        [TearDown]
        public void tearDown()
        {
        }


        //Test Case ID: SCR1
        [Test]
        public void testAdd()
        {
            //Arrange
            int count = repo.All().Count();
           

            // Act
            this.repo.Add(entity);
            this.repo.SaveChanges();
            // Assert

            Assert.NotNull(repo.GetByDetails(22,"ס"));

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();

        }



        //Test Case ID: SCR2
        [Test]
        public void testGetById()
        {
            // Arrange
            int count = repo.All().Count();

            SchoolClass c = repo.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            SchoolClass actual = repo.GetById(c.Id);
            // Assert

            Assert.NotNull(actual);

            //TODO add existing test
        }

        //Test Case ID: SCR3
        [Test]
        public void testGet()
        {
            // Arrange
            int count = repo.All().Count();
            this.repo.Add(entity);
            this.repo.SaveChanges();
               

            // Act
            SchoolClass actual = repo.Get(x => (x.ClassLetter == "ס" && x.ClassNumber == 22)).FirstOrDefault();

            // Assert

            Assert.NotNull(actual);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();

        }


        //Test Case ID: SCR4
        [Test]
        public void testUpdate()
        {

            // Arrange
            int count = repo.All().Count();
            repo.Add(entity);
            this.repo.SaveChanges();

            Assert.Null(repo.GetByDetails(99, "ס"));


            Assert.AreEqual(count + 1, repo.All().Count());
            entity.ClassNumber=99;


            // Act
            repo.Update(entity);
            repo.SaveChanges();

            // Assert

            Assert.NotNull(repo.GetByDetails(99,"ס"));
            this.repo.HardDelete(entity);
            this.repo.SaveChanges();

        }

        //Test Case ID: SCR5
        [Test]
        public void testDelete()
        {
            // Arrange
            repo.Add(entity);
            repo.SaveChanges();
            int count = repo.All().Count();

            Guid id = repo.GetByDetails(22, "ס").Id;
            // Act
            repo.Delete(entity);
            repo.SaveChanges();
            // Assert

            Assert.Null(repo.GetByDetails(22,"ס"));
            Assert.True(repo.All().Count() == count - 1);
            Assert.True(repo.GetById(id).IsDeleted);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();

            //TODO add all remaining methods
        }


        //Test Case ID: SCR6
        [Test]
        public void testGetByDetails()
        {
            // Arrange
            int count = repo.All().Count();
            this.repo.Add(entity);
            this.repo.SaveChanges();


            // Act
            SchoolClass actual = repo.GetByDetails(22,"ס");

            // Assert

            Assert.NotNull(actual);

        }
    }

}
