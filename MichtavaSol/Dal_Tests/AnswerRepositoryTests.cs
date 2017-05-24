
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
    class AnswerRepositoryTests
    {
        private ApplicationDbContext ctx;

        private AnswerRepository repo;

        private Answer entity;

        private const string n = "בדיקה בדיקה";

        private Guid studentId;

        private Guid hwId;

        [OneTimeSetUp]
        public void oneTimeSetUp()
        {
            var connection = DbConnectionFactory.CreateTransient();
            this.ctx = new ApplicationDbContext(connection);
            this.repo = new AnswerRepository(ctx);
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
            Student someStudent = this.ctx.Set<Student>().Include(x=>x.Homeworks).FirstOrDefault();
            Homework someHomework = someStudent.Homeworks.ElementAt(someStudent.Homeworks.Count()-1);
            studentId = someStudent.Id;
            hwId = someHomework.Id;

            entity = new Answer(someHomework, someStudent);
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

            Assert.NotNull(repo.GetByHomeworkIdAndStudentId(hwId,studentId));

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();
        }

        [Test]
        public void testGetById()
        {
            // Arrange
            int count = repo.All().Count();

            Answer c = repo.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Answer actual = repo.GetById(c.Id);
            // Assert

            Assert.NotNull(actual);

        }


        [Test]
        public void testUpdate()
        {

            // Arrange
            int count = repo.All().Count();
            repo.Add(entity);
            this.repo.SaveChanges();


            Guid id = repo.GetByHomeworkIdAndStudentId(hwId, studentId).Id;

            Assert.NotNull(id);
            Assert.AreEqual(count + 1, repo.All().Count());
            entity.TeacherFeedback = "alala";


            // Act
            repo.Update(entity);
            repo.SaveChanges();

            // Assert

            Assert.True(repo.GetById(id).TeacherFeedback == "alala");
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

            Guid id = repo.GetByHomeworkIdAndStudentId(hwId, studentId).Id;
            // Act
            repo.Delete(entity);
            repo.SaveChanges();
            // Assert

            Assert.True(repo.GetById(id).IsDeleted == true);
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
            Answer actual = repo.GetByHomeworkIdAndStudentId(hwId, studentId);

            // Assert

            Assert.NotNull(actual);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();
        }


        [Test]
        public void testGetByHwIdandStudentId()
        {
            // Arrange
            int count = repo.All().Count();
            this.repo.Add(entity);
            this.repo.SaveChanges();


            // Act
            Answer actual = repo.GetByHomeworkIdAndStudentId(hwId, studentId);

            // Assert

            Assert.NotNull(actual);

            this.repo.HardDelete(entity);
            this.repo.SaveChanges();
        }


    }

}
