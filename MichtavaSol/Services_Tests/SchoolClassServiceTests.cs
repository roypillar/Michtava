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
        public void testAddStandaloneSchoolClass()
        {
            Assert.Null(serv.GetByDetails(23, "ע"));
            // Arrange
            int count = serv.All().Count();


            // Act
            MichtavaResult res = serv.Add(entity);

            // Assert
            Assert.AreEqual(count + 1, serv.All().Count());
            Assert.NotNull(serv.GetByDetails(23, "ע"));
            Assert.True(res is MichtavaSuccess);
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }

        [Test]
        public void testAddSchoolClassExistingId()
        {
            Assert.Null(serv.GetByDetails(23, "ע"));
            // Arrange
            int count = serv.All().Count();
            SchoolClass existing = serv.All().FirstOrDefault();

            // Act
            MichtavaResult res = serv.Add(existing);

            // Assert
            Assert.True(res is MichtavaFailure);
        }

        [Test]
        public void testAddExistingSchoolClassDetails()
        {
            Assert.Null(serv.GetByDetails(23, "ע"));
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
            Assert.Null(serv.GetByDetails(23, "ע"));
        }


        [Test]
        public void testAddWithTeacherInconsistency()
        {
            Assert.Null(serv.GetByDetails(23, "ע"));
        }

        //Gets
        [Test]
        public void testGetSchoolClassByIdTrue()
        {
            Assert.Null(serv.GetByDetails(23, "ע"));
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
        public void testGetSchoolClassByIdFalse()
        {
            Assert.Null(serv.GetByDetails(23, "ע"));
            // Arrange
            int count = serv.All().Count();

            // Act
            SchoolClass actual = serv.GetById(Guid.NewGuid());
            // Assert

            Assert.Null(actual);

        }


        [Test]
        public void testGetSchoolClassByDetailsTrue()
        {
            Assert.Null(serv.GetByDetails(23, "ע"));
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
        public void testGetSchoolClassByDetailsFalse()
        {
            Assert.Null(serv.GetByDetails(23, "ע"));
            // Arrange
            int count = serv.All().Count();


            // Act
            SchoolClass actual = serv.GetByDetails(-1, "אסטערכן");
            // Assert

            Assert.Null(actual);

        }

        //Update
        [Test]
        public void testUpdateSchoolClassSuccess()
        {
            Assert.Null(serv.GetByDetails(23, "ע"));
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
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }

        [Test]
        public void testUpdateSchoolClassNonExistant()
        {
            Assert.Null(serv.GetByDetails(23, "ע"));
            // Arrange
            int count = serv.All().Count();

            Assert.Null(serv.GetByDetails(98, "ע"));


            entity.ClassNumber = 98;


            // Act
            MichtavaResult res = serv.Update(new SchoolClass(98, "ע"));

            // Assert
            Assert.True(res is MichtavaFailure);
        }

        //[Test]
        //public void testUpdateModifiedId()
        //{
        //    Assert.Null(serv.GetByDetails(23, "ע"));
        //    // Arrange
        //    int count = serv.All().Count();
        //    serv.Add(entity);

        //    Assert.Null(serv.GetByDetails(98, "ע"));


        //    Assert.AreEqual(count + 1, serv.All().Count());
        //    entity.setId(Guid.NewGuid());


        //    // Act
        //    MichtavaResult res = serv.Update(entity);

        //    // Assert
        //    Assert.True(res is MichtavaFailure);
        //    Assert.True(res.Message == "הכיתה לא נמצאה במערכת");

        //    Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        //}

        [Test]
        public void testUpdateSchoolClassExistingDetails()
        {
            Assert.Null(serv.GetByDetails(23, "ע"));
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
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }


        [Test]
        public void testUpdateSchoolClassAddSubject()
        {
            // Arrange
            int count = serv.All().Count();
            SchoolClass c = serv.All().FirstOrDefault();

            c.Subjects.Add(new Subject("test subj"));

        

            // Act
            MichtavaResult res = serv.Update(c);
            

            // Assert
            Assert.True(res is MichtavaSuccess);
        }

        //Deletes
        [Test]
        public void testDeleteSchoolClassSuccess()
        {
            Assert.Null(serv.GetByDetails(23, "ע"));
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

            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);


        }

        [Test]
        public void testDeleteSchoolClassIsStudentUpdated()
        {
            this.oneTimeSetUp();


            Assert.Null(serv.GetByDetails(23, "ע"));
            // Arrange
            int count = serv.All().Count();


            //SchoolClass c = serv.All().Include(x => x.Students).FirstOrDefault(y => y.Students.Count > 0);//DOES NOT WORK

            SchoolClass c = null;
            List<SchoolClass> cs = serv.All().Include(y => y.Students).ToList();//check
            foreach (SchoolClass sc in cs)
            {
                if (sc.Students.Count > 0)
                {
                    c = sc;
                    break;
                }
            }

            Assert.NotNull(c);

            ICollection<Student> studentsFromSchoolClass = c.Students;

          

            // Act
            MichtavaResult res = serv.Delete(c);






            // Assert

            List<Student> ss = this.ctx.Set<Student>().Include(x=>x.SchoolClass).ToList();//check
            List<Student> persistantStudentsFromSchoolClass = new List<Student>();

            foreach (Student s in ss)
            {
                if (containsId(studentsFromSchoolClass.Cast<HasId>().ToList(), s.Id))
                    persistantStudentsFromSchoolClass.Add(s);
            }
            Guid id = c.Id;

            Assert.True(res is MichtavaSuccess);
            Assert.True(serv.All().Count() == count - 1);
            Assert.True(serv.GetById(id).IsDeleted);




            foreach (Student s in persistantStudentsFromSchoolClass)
            {
                Assert.True(s.SchoolClass == null);
            }


        }


        [Test]
        public void testDeleteSchoolClassIsTeacherUpdated()
        {
            this.oneTimeSetUp();

            Assert.Null(serv.GetByDetails(23, "ע"));
            // Arrange
            int count = serv.All().Count();
            SchoolClass c=null;
            List<SchoolClass> cs = serv.All().Include(y => y.Teachers).ToList();
            foreach(SchoolClass sc in cs)
            {
                if (sc.Teachers.Count > 0)
                {
                    c = sc;
                    break;
                }
            }

            Assert.NotNull(c);

            ICollection<Teacher> teachersFromSchoolClass = c.Teachers;

            List<Teacher> ts = this.ctx.Set<Teacher>().Include(y=> y.SchoolClasses).ToList();//check
            List<Teacher> persistantTeachersFromSchoolClass = new List<Teacher>();

            foreach (Teacher t in ts) {
                if (containsId(teachersFromSchoolClass.Cast<HasId>().ToList(), t.Id))
                    persistantTeachersFromSchoolClass.Add(t);
            }
            Guid id = c.Id;

            // Act
            MichtavaResult res = serv.Delete(c);

            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.True(serv.All().Count() == count - 1);
            Assert.True(serv.GetById(id).IsDeleted);

            foreach (Teacher t in persistantTeachersFromSchoolClass)
            {
                Assert.False(containsId(t.SchoolClasses.Cast<HasId>().ToList(), id));
            }

            this.oneTimeSetUp();
        }

        private bool containsId(ICollection<HasId> col, Guid hId)
        {
            foreach (HasId h in col)
            {
                if (h.Id == hId)
                    return true;
            }
            return false;

        }

        [Test]
        public void testDeleteNonExistantSchoolClass()
        {
            Assert.Null(serv.GetByDetails(23, "ע"));
            // Arrange
            int count = serv.All().Count();

            entity.setId(Guid.NewGuid());
            Assert.Null(serv.GetById(entity.Id));

            // Act
            MichtavaResult res = serv.Delete(entity);


            // Assert
            Assert.True(res is MichtavaFailure);

        }



        [Test]
        public void testHardDeleteSuccessSchoolClass()
        {
            Assert.Null(serv.GetByDetails(23, "ע"));
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
            Assert.Null(serv.GetByDetails(23, "ע"));
            // Arrange
            serv.Add(entity);
            Student dummy = this.ctx.Set<Student>().FirstOrDefault(x => x.SchoolClass == null);


            // Act
            MichtavaResult res = serv.addStudentToSchoolClass(dummy, entity);


            // Assert
            Assert.True(res is MichtavaSuccess);

            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);



        }
        [Test]
        public void testAddToSchoolClassStudentUpdated()
        {
            Assert.Null(serv.GetByDetails(23, "ע"));
            // Arrange
            serv.Add(entity);
            Student dummy = this.ctx.Set<Student>().FirstOrDefault(x => x.SchoolClass == null);


            // Act
            MichtavaResult res = serv.addStudentToSchoolClass(dummy, entity);


            // Assert
            Assert.True(res is MichtavaSuccess);

            Assert.True(ctx.Set<Student>().Find(dummy.Id).SchoolClass.Id ==
                        serv.GetByDetails(23, "ע").Id);

            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);

        }

        [Test]
        public void testAddToSchoolClassSchoolClassUpdated()
        {
            Assert.Null(serv.GetByDetails(23, "ע"));
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

            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);

        }



    }
}
