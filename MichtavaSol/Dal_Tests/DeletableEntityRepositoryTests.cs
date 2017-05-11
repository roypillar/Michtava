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
    [TestFixture(typeof(Answer))]
    [TestFixture(typeof(Subject))]
    [TestFixture(typeof(Homework))]
    [TestFixture(typeof(Text))]
    [TestFixture(typeof(SchoolClass))]
    [TestFixture(typeof(Administrator))]
    [TestFixture(typeof(Teacher))]
    [TestFixture(typeof(Student))]
    //[TestFixture(typeof(ApplicationUser))]

    class DeletableEntityRepositoryTests<T> where T : DeletableEntity, new()
    {
        private IApplicationDbContext ctx;

        private DeletableEntityRepository<T> repo;

        private IDbSet<T> set;

        private T entity;

        private int _testId;


        public int TestId
        {
            get
            {
                _testId++;
                return _testId;
            }
        }

        [OneTimeSetUp]
        public void oneTimeSetUp()
        {
            _testId = 0;
            var connection = DbConnectionFactory.CreateTransient();
            this.ctx = new ApplicationDbContext(connection);
            this.repo = new DeletableEntityRepository<T>(ctx);

        }

        [OneTimeTearDown]
        public void oneTimeTearDown()
        {
            //nothing for now
        }


        [SetUp]
        public void setUp()
        {
            set = ctx.Set<T>();
            entity = new T();
            entity.setId(Guid.NewGuid());
            entity.TestID = TestId;
        }

        [TearDown]
        public void tearDown()
        {

        }


        [Test]
        public void testAdd()
        {
            // Arrange
            int count = set.Local.Count();
            // Act
            repo.Add(entity);

            // Assert
            Assert.AreEqual(entity.TestID, set.Local.FirstOrDefault().TestID);//general check

            Assert.AreEqual(count + 1, set.Local.Count());


            //TODO add existing test in Subjects
        }

        [Test]
        public void testRead()
        {
            // Arrange
            int count = set.Local.Count();
            repo.Add(entity);
            Assert.AreEqual(count + 1, set.Local.Count());


            // Act

            // Assert
            Assert.NotNull(repo.GetByTestID(entity.TestID));//this is fine, because of course the DB is empty beforehand.

            //TODO add existing test
        }


        [Test]
        public void testUpdate()
        {
            // Arrange
            int count = set.Local.Count();
            repo.Add(entity);
            Assert.AreEqual(count + 1, set.Local.Count());

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
            int count = set.Local.Count();
            repo.Add(entity);
            Assert.AreEqual(count + 1, set.Local.Count());

            // Act
            repo.Delete(entity);
            // Assert

            Assert.IsTrue(repo.GetByTestID(entity.TestID).IsDeleted);//this is fine, because of course the DB is empty beforehand.

            //TODO add all remaining methods
        }
    }

}
