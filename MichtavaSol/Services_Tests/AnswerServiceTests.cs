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
    class AnswerServiceTests
    {
        private ApplicationDbContext ctx;

        private AnswerService serv;

        private Answer entity;

        private  Guid hwId ;

        private  Guid studentId ;

        [OneTimeSetUp]
        public void oneTimeSetUp()
        {
            var connection = DbConnectionFactory.CreateTransient();
            this.ctx = new ApplicationDbContext(connection);
            this.serv = new AnswerService(new AnswerRepository(ctx));
            new DatabaseSeeder().CreateDependenciesAndSeed(ctx);//heavy duty

        }

        [SetUp]
        public void setUp()
        {
            Student s = ctx.Set<Student>().Include(y => y.Homeworks).Where(x => x.ApplicationUser.UserName == "callmeanna").FirstOrDefault();
            Guid id = s.Homeworks.ElementAt(1).Id;
            Homework hw = ctx.Set<Homework>().FirstOrDefault(x => x.Id == id);
            this.hwId = id;
            this.studentId = s.Id;
            entity = new Answer(hw, s);
        }

        //Adds
        //Test case ID : AS1
        [Test]
        public void testAddStandaloneAnswer()
        {
            Assert.Null(serv.GetByDetails(this.hwId, this.studentId));
            // Arrange
            int count = serv.All().Count();


            // Act
            MichtavaResult res = serv.Add(entity);

            // Assert
            Assert.AreEqual(count + 1, serv.All().Count());
            Assert.NotNull(serv.GetByDetails(this.hwId, this.studentId));
            Assert.True(res is MichtavaSuccess);
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }

        //Test case ID : AS2
        [Test]
        public void testAddExistingIdAnswer()
        {
            Assert.Null(serv.GetByDetails(this.hwId, this.studentId));
            // Arrange
            int count = serv.All().Count();
            Answer existing = serv.All().FirstOrDefault();

            // Act
            MichtavaResult res = serv.Add(existing);

            // Assert
            Assert.True(res is MichtavaFailure);
        }


        //Test case ID : AS3
        [Test]
        public void testAddExistingDetailsAnswer()
        {
            Assert.Null(serv.GetByDetails(this.hwId, this.studentId));
            // Arrange
            int count = serv.All().Count();
            Answer existing = serv.All().FirstOrDefault();

            Answer c = new Answer(existing.Answer_To, existing.Submitted_By);                
            // Act

            MichtavaResult res = serv.Add(c);

            // Assert
            Assert.True(res is MichtavaFailure);

        }



        //Test case ID : AS4
        [Test]
        public void testAddNullHomeworkAnswer()
        {
            Assert.Null(serv.GetByDetails(this.hwId, this.studentId));
            // Arrange
            int count = serv.All().Count();
            Answer existing = serv.All().FirstOrDefault();

            Answer c = new Answer();
            c.Submitted_By = ctx.Set<Student>().FirstOrDefault();

            // Act
            MichtavaResult res = serv.Add(c);

            // Assert
            Assert.True(res is MichtavaFailure);

        }


        //Test case ID : AS5
        [Test]
        public void testAddNullStudentAnswer()
        {
            Assert.Null(serv.GetByDetails(this.hwId, this.studentId));
            // Arrange
            int count = serv.All().Count();
            Answer existing = serv.All().FirstOrDefault();

            Answer c = new Answer();
            c.Answer_To = ctx.Set<Homework>().FirstOrDefault();

            // Act
            MichtavaResult res = serv.Add(c);

            // Assert
            Assert.True(res is MichtavaFailure);

        }

        //Gets


        //Test case ID : AS6
        [Test]
        public void testGetAnswerByIdTrue()
        {
            Assert.Null(serv.GetByDetails(this.hwId, this.studentId));
            // Arrange
            int count = serv.All().Count();

            Answer c = serv.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Answer actual = serv.GetById(c.Id);
            // Assert

            Assert.NotNull(actual);

        }



        //Test case ID : AS7
        [Test]
        public void testGetAnswerByIdFalse()
        {
            Assert.Null(serv.GetByDetails(this.hwId, this.studentId));
            // Arrange
            int count = serv.All().Count();

            // Act
            Answer actual = serv.GetById(Guid.NewGuid());
            // Assert

            Assert.Null(actual);

        }




        //Test case ID : AS7
        [Test]
        public void testGetAnswerByDetailsTrue()
        {
            Assert.Null(serv.GetByDetails(this.hwId, this.studentId));
            // Arrange
            int count = serv.All().Count();

            Answer c = serv.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Answer actual = serv.GetByDetails(c.Homework_Id, c.Student_Id);
            // Assert

            Assert.NotNull(actual);
        }





        //Test case ID : AS8
        [Test]
        public void testGetAnswerByDetailsFalse()
        {
            Assert.Null(serv.GetByDetails(this.hwId, this.studentId));
            // Arrange
            int count = serv.All().Count();


            // Act
            Answer actual = serv.GetByDetails(Guid.NewGuid(), Guid.NewGuid());
            // Assert

            Assert.Null(actual);

        }

        //Update


        //Test case ID : AS9
        [Test]
        public void testUpdateAnswerSuccess()
        {
            Assert.Null(serv.GetByDetails(this.hwId, this.studentId));
            // Arrange
            int count = serv.All().Count();
            serv.Add(entity);

            Assert.AreEqual(count + 1, serv.All().Count());
            entity.TeacherFeedback = "test feedback";
            Guid id = serv.GetByDetails(this.hwId, this.studentId).Id;


            // Act
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.NotNull(serv.GetById(id));
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }






        //Test case ID : AS9
        [Test]
        public void testUpdateAnswerNullHomework()
        {
            Assert.Null(serv.GetByDetails(this.hwId, this.studentId));
            // Arrange
            int count = serv.All().Count();
            Answer c = serv.All().FirstOrDefault();

            serv.Add(entity);

            Assert.AreEqual(count + 1, serv.All().Count());

            entity.Answer_To = null;

            // Act
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaFailure);
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }



        //Test case ID : AS10
        [Test]
        public void testUpdateAnswerNullStudent()
        {
            Assert.Null(serv.GetByDetails(this.hwId, this.studentId));
            // Arrange
            int count = serv.All().Count();
            Answer c = serv.All().FirstOrDefault();

            serv.Add(entity);

            Assert.AreEqual(count + 1, serv.All().Count());

            entity.Submitted_By = null;

            // Act
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaFailure);
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }

        //Deletes


        //Test case ID : AS11
        [Test]
        public void testDeleteAnswerSuccess()
        {
            Assert.Null(serv.GetByDetails(this.hwId, this.studentId));
            // Arrange
            serv.Add(entity);
            int count = serv.All().Count();

            Guid id = serv.GetByDetails(this.hwId, this.studentId).Id;

            // Act
            MichtavaResult res = serv.Delete(entity);


            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.Null(serv.GetByDetails(this.hwId, this.studentId));
            Assert.True(serv.All().Count() == count - 1);
            Assert.True(serv.GetById(id).IsDeleted);

            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);


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



        //Test case ID : AS12
        [Test]
        public void testDeleteNonExistant()
        {
            Assert.Null(serv.GetByDetails(this.hwId, this.studentId));
            // Arrange
            int count = serv.All().Count();

            entity.setId(Guid.NewGuid());
            Assert.Null(serv.GetById(entity.Id));

            // Act
            MichtavaResult res = serv.Delete(entity);


            // Assert
            Assert.True(res is MichtavaFailure);

        }



        //Test case ID : AS13
        [Test]
        public void testHardDeleteSuccessAnswer()
        {
            Assert.Null(serv.GetByDetails(this.hwId, this.studentId));
            // Arrange
            serv.Add(entity);
            int count = serv.All().Count();

            Guid id = serv.GetByDetails(this.hwId, this.studentId).Id;

            // Act
            MichtavaResult res = serv.HardDelete(entity);


            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.Null(serv.GetByDetails(this.hwId, this.studentId));
            Assert.True(serv.All().Count() == count - 1);
        }

        //Customs





        //add question, remove question, quickly tho

    }
}
