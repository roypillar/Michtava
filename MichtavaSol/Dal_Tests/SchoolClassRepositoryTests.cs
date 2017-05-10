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
  
    [TestFixture]
    class SchoolClassRepositoryTests
    {
        private IApplicationDbContext ctx;

        private SchoolClassRepository repo;

        private IDbSet<SchoolClass> set;

        private SchoolClass entity;

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
            this.repo = new SchoolClassRepository(ctx);
            this.repo.SaveChanges();

        }

        [OneTimeTearDown]
        public void oneTimeTearDown()
        {
            //nothing for now
        }


        [SetUp]
        public void setUp()
        {
            set = ctx.Set<SchoolClass>();
            entity = new SchoolClass();
            entity.setId(Guid.NewGuid());
            entity.TestID = TestId;
        }

        [TearDown]
        public void tearDown()
        {

        }


        [Test]
        public void testGetByDetails()
        {
            // Arrange
            int count = set.Local.Count();
            entity.ClassLetter = "ט";
            entity.ClassNumber = 7;

            repo.Add(entity);
            Assert.AreEqual(count + 1, set.Local.Count());


            // Act
            // Assert
            Assert.NotNull(repo.GetByTestID(entity.TestID));//this is fine, because of course the DB is empty beforehand.
            Assert.NotNull(repo.GetByDetails(7,"ט"));

        }


    }

}
