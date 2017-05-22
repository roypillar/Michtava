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
            this.serv = new SchoolClassService(new SchoolClassRepository(ctx),
                                               new StudentRepository(ctx),
                                               new TeacherRepository(ctx));
            new DatabaseSeeder().CreateDependenciesAndSeed(ctx);//heavy duty

        }

        [SetUp]
        public void setUp()
        {
            entity = new SchoolClass(23, "ע");
        }

        //Adds
        [Test]
        public void testAddStandalone()
        {
            // Arrange
            int count = serv.All().Count();


            // Act
            MichtavaResult res = serv.Add(entity);

            // Assert
            Assert.AreEqual(count + 1, serv.All().Count());
            Assert.NotNull(serv.GetByDetails(23, "ע"));
            Assert.True(res is MichtavaSuccess);
            serv.HardDelete(entity);
        }

        [Test]
        public void testAddExistingId()
        {
            // Arrange
            int count = serv.All().Count();
            SchoolClass existing = serv.All().FirstOrDefault();

            // Act
            MichtavaResult res = serv.Add(existing);

            // Assert
            Assert.True(res is MichtavaFailure);
        }

        [Test]
        public void testAddExistingDetails()
        {
            // Arrange
            int count = serv.All().Count();
            SchoolClass existing = serv.All().FirstOrDefault();
            SchoolClass c = new SchoolClass(existing.ClassNumber, existing.ClassLetter);
            // Act

            MichtavaResult res = serv.Add(c);

            // Assert
            Assert.True(res is MichtavaFailure);
        }

        [Test]
        public void testAddWithStudentInconsistency()
        {

        }


        [Test]
        public void testAddWithTeacherInconsistency()
        {

        }

        //Gets
        [Test]
        public void testGetByIdTrue()
        {
            // Arrange
            int count = serv.All().Count();

            SchoolClass c = serv.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            SchoolClass actual = serv.GetById(c.Id);
            // Assert

            Assert.NotNull(actual);

        }

        [Test]
        public void testGetByIdFalse()
        {
            // Arrange
            int count = serv.All().Count();

            // Act
            SchoolClass actual = serv.GetById(Guid.NewGuid());
            // Assert

            Assert.Null(actual);

        }


        [Test]
        public void testGetByDetailsTrue()
        {
            // Arrange
            int count = serv.All().Count();

            SchoolClass c = serv.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            SchoolClass actual = serv.GetByDetails(c.ClassNumber, c.ClassLetter);
            // Assert

            Assert.NotNull(actual);
        }



        [Test]
        public void testGetByDetailsFalse()
        {
            // Arrange
            int count = serv.All().Count();


            // Act
            SchoolClass actual = serv.GetByDetails(-1, "אסטערכן");
            // Assert

            Assert.Null(actual);

        }

        //Update
        [Test]
        public void testUpdateSuccess()
        {
            // Arrange
            int count = serv.All().Count();
            serv.Add(entity);

            Assert.Null(serv.GetByDetails(98, "ע"));


            Assert.AreEqual(count + 1, serv.All().Count());
            entity.ClassNumber = 98;


            // Act
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.NotNull(serv.GetByDetails(98, "ע"));
            this.serv.HardDelete(entity);
        }

        [Test]
        public void testUpdateNonExistant()
        {
            // Arrange
            int count = serv.All().Count();

            Assert.Null(serv.GetByDetails(98, "ע"));


            entity.ClassNumber = 98;


            // Act
            MichtavaResult res = serv.Update(new SchoolClass(98, "ע"));

            // Assert
            Assert.True(res is MichtavaFailure);
        }

        [Test]
        public void testUpdateModifiedId()
        {
            // Arrange
            int count = serv.All().Count();
            serv.Add(entity);

            Assert.Null(serv.GetByDetails(98, "ע"));


            Assert.AreEqual(count + 1, serv.All().Count());
            entity.setId(Guid.NewGuid());


            // Act
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaFailure);
            Assert.True(res.Message == "הכיתה לא נמצאה במערכת");

            this.serv.HardDelete(entity);
        }

        [Test]
        public void testUpdateExistingDetails()
        {
            // Arrange
            int count = serv.All().Count();
            SchoolClass c = serv.All().FirstOrDefault();

            serv.Add(entity);

            Assert.Null(serv.GetByDetails(98, "ע"));


            Assert.AreEqual(count + 1, serv.All().Count());
            entity.ClassNumber = c.ClassNumber;
            entity.ClassLetter = c.ClassLetter;


            // Act
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaFailure);
            this.serv.HardDelete(entity);
        }

        //Deletes
        [Test]
        public void testDeleteSuccess()
        {
            // Arrange
            serv.Add(entity);
            int count = serv.All().Count();

            Guid id = serv.GetByDetails(23, "ע").Id;

            // Act
            MichtavaResult res = serv.Delete(entity);


            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.Null(serv.GetByDetails(23, "ע"));
            Assert.True(serv.All().Count() == count - 1);
            Assert.True(serv.GetById(id).IsDeleted);

            this.serv.HardDelete(entity);


        }

        [Test]
        public void testDeleteIsStudentUpdated()
        {
            // Arrange
            int count = serv.All().Count();
            SchoolClass c = serv.All().Include(x => x.Students).FirstOrDefault(y => y.Students.Count > 0);//check
            ICollection<Student> students = c.Students;
            Guid id = c.Id;

            // Act
            MichtavaResult res = serv.Delete(c);


            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.True(serv.All().Count() == count - 1);
            Assert.True(serv.GetById(id).IsDeleted);

            foreach (Student s in students)
            {
                Assert.True(s.SchoolClass == null);
            }

            this.oneTimeSetUp();

        }

             [Test]
        public void testDeleteIsTeacherUpdated()
        {
            // Arrange
            int count = serv.All().Count();
            SchoolClass c = serv.All().Include(x => x.Teachers).FirstOrDefault(y => y.Teachers.Count > 0);//check
            ICollection<Teacher> teachersFromSchoolClass = c.Teachers;

            var teachers = this.ctx.Set<Teacher>().Where(x => teachersContainsId(teachersFromSchoolClass, x.Id)).Include(y => y.SchoolClasses);

            Guid id = c.Id;

            // Act
            MichtavaResult res = serv.Delete(c);


            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.True(serv.All().Count() == count - 1);
            Assert.True(serv.GetById(id).IsDeleted);

            foreach (Teacher t in teachers)
            {
                Assert.False(schoolClassContainsId(t.SchoolClasses,id));
            }

            this.oneTimeSetUp();
        }

        private bool teachersContainsId(ICollection<Teacher> teachers, Guid tId)
        {
            foreach (Teacher t in teachers)
            {
                if (t.Id == tId)
                    return true;
            }
            return false;

        }

        private bool schoolClassContainsId(ICollection<SchoolClass> scs, Guid sId)
        {
            foreach (SchoolClass sc in scs)
            {
                if (sc.Id == sId)
                    return true;
            }
            return false;
        }

        [Test]
        public void testDeleteNonExistant()
        {
            // Arrange
            int count = serv.All().Count();

            Guid id = serv.GetByDetails(23, "ע").Id;
            entity.setId(Guid.NewGuid());
            Assert.Null(serv.GetById(entity.Id));

            // Act
            MichtavaResult res = serv.Delete(entity);


            // Assert
            Assert.True(res is MichtavaFailure);

        }



        [Test]
        public void testHardDeleteSuccess()
        {
            // Arrange
            serv.Add(entity);
            int count = serv.All().Count();

            Guid id = serv.GetByDetails(23, "ע").Id;

            // Act
            MichtavaResult res = serv.HardDelete(entity);


            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.Null(serv.GetByDetails(23, "ע"));
            Assert.True(serv.All().Count() == count - 1);
        }

        //Customs
        [Test]
        public void testAddToSchoolClassSuccess()
        {
            // Arrange
            serv.Add(entity);
            Student dummy = this.ctx.Set<Student>().FirstOrDefault(x => x.SchoolClass == null);


            // Act
            MichtavaResult res = serv.addStudentToSchoolClass(dummy, entity);


            // Assert
            Assert.True(res is MichtavaSuccess);

            


        }
        [Test]
        public void testAddToSchoolClassStudentUpdated()
        {
            // Arrange
            serv.Add(entity);
            Student dummy = this.ctx.Set<Student>().FirstOrDefault(x => x.SchoolClass == null);


            // Act
            MichtavaResult res = serv.addStudentToSchoolClass(dummy, entity);


            // Assert
            Assert.True(res is MichtavaSuccess);

            Assert.True(ctx.Set<Student>().Find(dummy.Id).SchoolClass.Id ==
                        serv.GetByDetails(23, "ע").Id);


        }

        [Test]
        public void testAddToSchoolClassSchoolClassUpdated()
        {
            // Arrange
            serv.Add(entity);
            Student dummy = this.ctx.Set<Student>().FirstOrDefault(x => x.SchoolClass == null);


            // Act
            MichtavaResult res = serv.addStudentToSchoolClass(dummy, entity);


            // Assert
            Assert.True(res is MichtavaSuccess);

            Assert.True(serv.All().Where(x => x.ClassNumber == entity.ClassNumber &&
                                                x.ClassLetter == entity.ClassLetter)
                                                .Include(y => y.Students).FirstOrDefault().Students.Contains(dummy));


        }



    }
}
