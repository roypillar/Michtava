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

namespace Dal_Tests
{
    [TestFixture(typeof(Answer))]
    [TestFixture(typeof(Subject))]
    class GenericRepositoryTests<T> where T : class, new()
    {
        private IApplicationDbContext ctx;

        private GenericRepository<T> repo;

        private IDbSet<T> set;

        private T entity;

        [OneTimeSetUp]
        void oneTimeSetUp()
        {
            this.ctx = ApplicationDbContext.Create();

        }

        [OneTimeTearDown]
        void oneTimeTearDown()
        {
            //nothing for now
        }


        [SetUp]
        void setUp()
        {
            repo = new GenericRepository<T>(ctx);
            set = ctx.Set<T>();
            entity = new T();
        }

        [TearDown]
        void tearDown()
        {
            set = null;
            entity = null;
        }


        [Test]
        public void testCreate()
        {
            // Arrange
            int count = set.Count();
            // Act
            repo.Add(entity);

            // Assert
            Assert.AreEqual(set.Count(), count+1);
        }
    }
    
}
