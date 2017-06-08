
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
    class TextRepositoryTests
    {
        private ApplicationDbContext ctx;

        private TextRepository repo;

        private Text entity;

        private const string n = "בדיקה בדיקה";

        [OneTimeSetUp]
        public void oneTimeSetUp()
        {
            var connection = DbConnectionFactory.CreateTransient();
            this.ctx = new ApplicationDbContext(connection);
            this.repo = new TextRepository(ctx);
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
            entity = new Text(n,this.ctx.Set<Subject>().FirstOrDefault());
        }

        [TearDown]
        public void tearDown()
        {
        }



        //Test case ID : TxR1
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


        //Test case ID : TxR2
        [Test]
        public void testGetById()
        {
            // Arrange
            int count = repo.All().Count();

            Text c = repo.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Text actual = repo.GetById(c.Id);
            // Assert

            Assert.NotNull(actual);

        }


        //Test case ID : TxR3
        [Test]
        public void testGet()
        {
            // Arrange
            int count = repo.All().Count();
            this.repo.Add(entity);
            this.repo.SaveChanges();


            // Act
            Text actual = repo.Get(x => (x.Name == n)).FirstOrDefault();

            // Assert

            Assert.NotNull(actual);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();

        }


        //Test case ID : TxR4
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


        //Test case ID : TxR5
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

        //Test case ID : TxR8
        [Test]
        public void testHardDelete()
        {
            // Arrange
            repo.Add(entity);
            repo.SaveChanges();
            int count = repo.All().Count();

            Guid id = repo.GetByName(n).Id;
            // Act
            repo.HardDelete(entity);
            repo.SaveChanges();
            // Assert

            Assert.Null(repo.GetByName(n));
            Assert.True(repo.All().Count() == count - 1);

          
        }



        //Test case ID : TxR6
        [Test]
        public void testGetByDetails()
        {
            // Arrange
            int count = repo.All().Count();
            this.repo.Add(entity);
            this.repo.SaveChanges();


            // Act
            Text actual = repo.GetByName(n);

            // Assert

            Assert.NotNull(actual);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();
        }



        //Test case ID : TxR7
        [Test]
        public void testGetByName()
        {
            // Arrange
            int count = repo.All().Count();
            this.repo.Add(entity);
            this.repo.SaveChanges();


            // Act
            Text actual = repo.GetByName(n);

            // Assert

            Assert.NotNull(actual);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();
        }


    }

}
