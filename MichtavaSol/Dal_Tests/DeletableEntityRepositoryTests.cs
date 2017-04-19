﻿using Dal;
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

namespace Dal_Tests
{
    [TestFixture(typeof(Answer))]
    [TestFixture(typeof(Subject))]
    class DeletableEntityRepositoryTests<T> where T : class, IDeletableEntity, new()
    {
        private IApplicationDbContext ctx;

        private DeletableEntityRepository<T> repo;

        private IDbSet<T> set;

        private T entity;

        [OneTimeSetUp]
        public void oneTimeSetUp()
        {
            
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
        public  void setUp()
        {
            set = ctx.Set<T>();
            entity = new T();
        }

        [TearDown]
        public void tearDown()
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
            Assert.AreEqual(count+1, set.Local.Count());

            //TODO add existing test

            //Clean
            repo.HardDelete(entity);
        }
    }
    
}
