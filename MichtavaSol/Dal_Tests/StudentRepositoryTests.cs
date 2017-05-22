
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
    class StudentRepositoryTests
    {
        private ApplicationDbContext ctx;

        private StudentRepository repo;

        private Student entity;

        private const string n = "  סטודנט בדיקה";

        [OneTimeSetUp]
        public void oneTimeSetUp()
        {
            var connection = DbConnectionFactory.CreateTransient();
            this.ctx = new ApplicationDbContext(connection);
            this.repo = new StudentRepository(ctx);
            new DatabaseSeeder().CreateDependenciesAndSeed(ctx);//heavy duty

        }

        [OneTimeTearDown]
        public void oneTimeTearDown()
        {
        }


        [SetUp]
        public void setUp()
        {
            entity = new Student(n);
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

            Assert.True(repo.Count() == count + 1);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();

        }

        [Test]
        public void testGetById()
        {
            // Arrange
            int count = repo.All().Count();

            Student c = repo.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Student actual = repo.GetById(c.Id);
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
            Student actual = repo.Get(x =>  x.Name == n).FirstOrDefault();

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

            Assert.IsEmpty(repo.Get(x => x.Name == n + "alala"));


            Assert.AreEqual(count + 1, repo.All().Count());
            entity.Name += "alala";


            // Act
            repo.Update(entity);
            repo.SaveChanges();

            // Assert

            Assert.NotNull(repo.Get(x => x.Name == n + "alala"));

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

            Guid id = repo.Get(x => x.Name == n).FirstOrDefault().Id;

            // Act
            repo.Delete(entity);
            repo.SaveChanges();
            // Assert

            Assert.IsEmpty(repo.Get(x => x.Name == n + "alala" && x.IsDeleted == false));

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
            Student actual = repo.Get(x => x.Name == n).FirstOrDefault();

            // Assert

            Assert.NotNull(actual);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();
        }

        [Test]
        public async Task testGetByUserName()
        {
            // Arrange
            int count = repo.All().Count();
            this.repo.Add(entity);
            this.repo.SaveChanges();

            Student some = this.ctx.Set<Student>().FirstOrDefault();

            // Act
            Student actual = await repo.GetByUserName(some.ApplicationUser.UserName);

            // Assert

            Assert.NotNull(actual);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();
        }

        [Test]
        public void testSearchByName()
        {
            // Arrange
            int count = repo.All().Count();
            this.repo.Add(entity);
            this.repo.SaveChanges();


            // Act
            IQueryable<Student> actual =  repo.SearchByName(n);

            // Assert

            Assert.True(actual.Count() >= 1);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();
        }

        [Test]
        public void testIsUserNameUniqueOnEditFailure()
        {

            Student some = this.ctx.Set<Student>().FirstOrDefault();
            Student other = this.ctx.Set<Student>().Where(x => x.Id != some.Id).FirstOrDefault();

            Assert.AreNotEqual(some.Id, other.Id);

            Assert.False(repo.IsUserNameUniqueOnEdit(other, some.ApplicationUser.UserName));

        }

        [Test]
        public void testIsUserNameUniqueOnEditSuccess()
        {

            Student some = this.ctx.Set<Student>().FirstOrDefault();


            Assert.True(repo.IsUserNameUniqueOnEdit(some, "זרבובן4"));
        }


        [Test]
        public void testIsEmailUniqueOnEditFailure()
        {
            Student some = this.ctx.Set<Student>().FirstOrDefault();
            Student other = this.ctx.Set<Student>().Where(x => x.Id != some.Id).FirstOrDefault();

            Assert.AreNotEqual(some.Id, other.Id);

            Assert.False(repo.IsEmailUniqueOnEdit(other, some.ApplicationUser.Email));
        }

        [Test]
        public void testIsEmailUniqueOnEditSuccess()
        {
            Student other = this.ctx.Set<Student>().FirstOrDefault();


            Assert.True(repo.IsEmailUniqueOnEdit(other, "rere@gmail.tootsies"));
        }



    }

}
