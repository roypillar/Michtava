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
            this.serv = new SchoolClassService(new SchoolClassRepository(ctx), new StudentRepository(ctx));
            new DatabaseSeeder().CreateDependenciesAndSeed(ctx);//heavy duty

        }

        [SetUp]
        public void setUp()
        {
            entity = new SchoolClass(23, "ע");
        }

      

        [Test]
        public void testAddToSchoolClassSuccess()
        {
            // Arrange
            serv.Add(entity);
            Student dummy = this.ctx.Set<Student>().FirstOrDefault(x => x.SchoolClass == null);


            // Act
            MichtavaResult res = serv.addStudentToSchoolClass(dummy, entity);


            // Assert
            Assert.IsInstanceOf(typeof(MichtavaSuccess),res);

            Assert.True(ctx.Set<Student>().Find(dummy.Id).SchoolClass.Id ==
                        serv.GetByDetails(23, "ע").Id);


        }

        [Test]
        public void testAddStandalone()
        {
            // Arrange
            int count = serv.All().Count();


            // Act
            serv.Add(entity);

            // Assert
            Assert.AreEqual(count + 1, serv.All().Count());
            Assert.NotNull(serv.GetByDetails(23, "ע"));
        }
    }
}
