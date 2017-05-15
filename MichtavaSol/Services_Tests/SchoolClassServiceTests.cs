using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Entities.Models;
using System.Data.Common;
using Effort;
using Common;
using Dal;
using Services;
using Dal.Repositories;
using NUnit.Framework;

namespace Services_Tests
{
    class SchoolClassServiceTests
    {
        private ApplicationDbContext ctx;

        private SchoolClassService serv;

        private SchoolClass entity;

        [OneTimeSetUp]
        public void oneTimeSetUp()
        {
            var connection = DbConnectionFactory.CreateTransient();
            this.ctx = new ApplicationDbContext(connection);
            this.serv = new SchoolClassService(new SchoolClassRepository(ctx));
            new DatabaseSeeder().CreateDependenciesAndSeed(ctx);//heavy duty

        }

        [Test]
        public void testAddToSchoolClass()
        {
            // Arrange
            int count = serv.All().Count();
            serv.Add(entity);
            this.ctx.Set<Student>().FirstOrDefault(x => x.SchoolClass != null);


            // Act

            // Assert
            Assert.AreEqual(count + 1, serv.All().Count());


            //TODO add existing test in Subjects
        }
    }
}
