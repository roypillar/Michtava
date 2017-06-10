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
    class HomeworkServiceTests
    {
        private ApplicationDbContext ctx;

        private HomeworkService serv;

        private Homework entity;

        private const string hwTitl = "טסטינגטון";

        private const string hwDesc = "555טסטינגטון";

        [OneTimeSetUp]
        public void oneTimeSetUp()
        {
            var connection = DbConnectionFactory.CreateTransient();
            this.ctx = new ApplicationDbContext(connection);
            this.serv = new HomeworkService(new HomeworkRepository(ctx));
            new DatabaseSeeder().CreateDependenciesAndSeed(ctx);//heavy duty

        }

        [SetUp]
        public void setUp()
        {
            entity = new Homework(hwTitl, hwDesc, DateTime.Now,
                 ctx.Set<Teacher>().FirstOrDefault(),
                 ctx.Set<Text>().FirstOrDefault());
        }

        //Adds

        //Test Case ID: HWS1
        [Test]
        public void testAddStandaloneHomework()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();


            // Act
            MichtavaResult res = serv.Add(entity);

            // Assert
            Assert.AreEqual(count + 1, serv.All().Count());
            Assert.NotNull(serv.GetByDetails(hwTitl, hwDesc));
            Assert.True(res is MichtavaSuccess);
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }


        //Test Case ID: HWS2
        [Test]
        public void testAddExistingIdHomework()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();
            Homework existing = serv.All().FirstOrDefault();

            // Act
            MichtavaResult res = serv.Add(existing);

            // Assert
            Assert.True(res is MichtavaFailure);
        }

        //Test Case ID: HWS3
        [Test]
        public void testAddExistingDetailsHomework()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();
            Homework existing = serv.All().FirstOrDefault();

            Homework c = new Homework(existing.Title, existing.Description, DateTime.Now,
                 ctx.Set<Teacher>().FirstOrDefault(),
                 ctx.Set<Text>().FirstOrDefault());
            // Act

            MichtavaResult res = serv.Add(c);

            // Assert
            Assert.True(res is MichtavaFailure);

        }


        //Test Case ID: HWS4
        [Test]
        public void testAddNullTextHomework()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();
            Homework existing = serv.All().FirstOrDefault();

            Homework c = new Homework();
            c.Title = hwTitl;
            c.Description = hwDesc;
            c.Deadline = DateTime.Now;
            c.Created_By = ctx.Set<Teacher>().FirstOrDefault();
    
            // Act
            MichtavaResult res = serv.Add(c);

            // Assert
            Assert.True(res is MichtavaFailure);

        }


        //Test Case ID: HWS5
        [Test]
        public void testAddNullTeacherHomework()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();
            Homework existing = serv.All().FirstOrDefault();

            Homework c = new Homework();
            c.Title = hwTitl;
            c.Description = hwDesc;
            c.Deadline = DateTime.Now;
            c.Text = ctx.Set<Text>().FirstOrDefault();

            // Act
            MichtavaResult res = serv.Add(c);

            // Assert
            Assert.True(res is MichtavaFailure);

        }


        //Test Case ID: HWS6
        [Test]
        public void testAddNullDateHomework()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();
            Homework existing = serv.All().FirstOrDefault();

            Homework c = new Homework();
            c.Title = hwTitl;
            c.Description = hwDesc;
            c.Text = ctx.Set<Text>().FirstOrDefault();
            c.Created_By = ctx.Set<Teacher>().FirstOrDefault();
            //c.Teacher_Id = c.Created_By.Id;
            //c.Text_Id = c.Text.Id;
            // Act
            MichtavaResult res = serv.Add(c);

            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.NotNull(serv.GetByDetails(hwTitl, hwDesc).Deadline);
            Assert.True(serv.HardDelete(c) is MichtavaSuccess);
        }

        //Gets
        //Test Case ID: HWS7
        [Test]
        public void testGetHomeworkByIdTrue()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();

            Homework c = serv.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Homework actual = serv.GetById(c.Id);
            // Assert

            Assert.NotNull(actual);

        }


        //Test Case ID: HWS8
        [Test]
        public void testGetHomeworkByIdFalse()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();

            // Act
            Homework actual = serv.GetById(Guid.NewGuid());
            // Assert

            Assert.Null(actual);

        }


        //Test Case ID: HWS9
        [Test]
        public void testGetHomeworkByDetailsTrue()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();

            Homework c = serv.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Homework actual = serv.GetByDetails(c.Title, c.Description);
            // Assert

            Assert.NotNull(actual);
        }



        //Test Case ID: HWS10
        [Test]
        public void testGetHomeworkByDetailsFalse()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();


            // Act
            Homework actual = serv.GetByDetails("14", "אסטערכן");
            // Assert

            Assert.Null(actual);

        }

        //Update

        //Test Case ID: HWS11
        [Test]
        public void testUpdateHomeworkSuccess()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();
            serv.Add(entity);

            Assert.Null(serv.GetByDetails("98", hwDesc));


            Assert.AreEqual(count + 1, serv.All().Count());
            entity.Title = "98";


            // Act
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.NotNull(serv.GetByDetails("98", hwDesc));
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }

        //Test Case ID: HWS11.1
        [Test]
        public void testUpdateHomeworkAddQuestionSuccess()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();
            serv.Add(entity);

            Guid id = serv.GetByDetails(hwTitl, hwDesc).Id;
            Assert.AreEqual(count + 1, serv.All().Count());
            entity.Questions.Add(new Question("test question"));

            // Act
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaSuccess);
            var quests = serv.All().Include(x => x.Questions).FirstOrDefault(y => y.Id == id).Questions.ToList();
            Assert.NotNull(quests);
            Assert.True(quests.Count() != 0);
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }

        //Test Case ID: HWS11.2
        [Test]
        public void testUpdateHomeworkEditQuestionSuccess()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();
            serv.Add(entity);
            entity.Questions.Add(new Question("test question"));
            Assert.True(serv.Update(entity) is MichtavaSuccess);

            Guid id = serv.GetByDetails(hwTitl, hwDesc).Id;
            Assert.AreEqual(count + 1, serv.All().Count());

            // Act
            var quest = serv.All().Include(x => x.Questions).FirstOrDefault(y => y.Id == id).Questions.ElementAt(0);
            quest.Content = "edited question";
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaSuccess);
            var quests = serv.All().Include(x => x.Questions).FirstOrDefault(y => y.Id == id).Questions.ToList();
            Assert.NotNull(quests);
            Assert.True(quests.Count() != 0);
            Assert.True(quests.ElementAt(0).Content == "edited question");
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }

        //Test Case ID: HWS11.3
        [Test]
        public void testUpdateHomeworkRemoveQuestionSuccess()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();
            serv.Add(entity);
            Question q = new Question("test question");
            entity.Questions.Add(q);
            Assert.True(serv.Update(entity) is MichtavaSuccess);

            Guid id = serv.GetByDetails(hwTitl, hwDesc).Id;
            Assert.AreEqual(count + 1, serv.All().Count());

            // Act
            serv.All().Include(x => x.Questions).FirstOrDefault(y => y.Id == id).Questions.Remove(q);

            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaSuccess);
            var quests = serv.All().Include(x => x.Questions).FirstOrDefault(y => y.Id == id).Questions.ToList();
            Assert.NotNull(quests);
            Assert.True(quests.Count() == 0);
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }

        //Test Case ID: HWS11.4
        [Test]
        public void testUpdateHomeworkAddKSToQuestionSuccess()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();
            serv.Add(entity);
            entity.Questions.Add(new Question("test question"));
            Assert.True(serv.Update(entity) is MichtavaSuccess);

            Guid id = serv.GetByDetails(hwTitl, hwDesc).Id;
            Assert.AreEqual(count + 1, serv.All().Count());

            // Act
            var quest = serv.All().Include(x => x.Questions).FirstOrDefault(y => y.Id == id).Questions.ElementAt(0);
            quest.Suggested_Openings.Add(new SuggestedOpening("TSO"));
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaSuccess);
            var quests = serv.All().Include(x => x.Questions).FirstOrDefault(y => y.Id == id).Questions.ToList();
            Assert.NotNull(quests);
            Assert.True(quests.Count() != 0);
            Assert.True(quests.ElementAt(0).Suggested_Openings.Count()!=0);
            Assert.True(quests.ElementAt(0).Suggested_Openings.ElementAt(0).Content == "TSO");

            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }
        //Test Case ID: HWS11.5
        [Test]
        public void testUpdateHomeworkEditKSTFromQuestionSuccess()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();
            serv.Add(entity);
            Question q = new Question("test question");
            SuggestedOpening so = new SuggestedOpening("TSO");
            q.Suggested_Openings.Add(so);
            entity.Questions.Add(q);
            Assert.True(serv.Update(entity) is MichtavaSuccess);

            Guid id = serv.GetByDetails(hwTitl, hwDesc).Id;
            Assert.AreEqual(count + 1, serv.All().Count());

            // Act
            var quest = serv.All().Include(x => x.Questions).FirstOrDefault(y => y.Id == id).Questions.ElementAt(0);
            quest.Suggested_Openings.ElementAt(0).Content = "EDITED";
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaSuccess);
            var quests = serv.All().Include(x => x.Questions).FirstOrDefault(y => y.Id == id).Questions.ToList();
            Assert.NotNull(quests);
            Assert.True(quests.Count() != 0);
            Assert.True(quests.ElementAt(0).Suggested_Openings.Count() != 0);
            Assert.True(quests.ElementAt(0).Suggested_Openings.ElementAt(0).Content == "EDITED");
           Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }

        //Test Case ID: HWS11.6
        [Test]
        public void testUpdateHomeworkRemoveKSTFromQuestionSuccess()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();
            serv.Add(entity);
            Question q = new Question("test question");
            SuggestedOpening so = new SuggestedOpening("TSO");
            q.Suggested_Openings.Add(so);
            entity.Questions.Add(q);
            Assert.True(serv.Update(entity) is MichtavaSuccess);

            Guid id = serv.GetByDetails(hwTitl, hwDesc).Id;
            Assert.AreEqual(count + 1, serv.All().Count());

            // Act
            var quest = serv.All().Include(x => x.Questions).FirstOrDefault(y => y.Id == id).Questions.ElementAt(0);
            quest.Suggested_Openings.Remove(so);
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaSuccess);
            var quests = serv.All().Include(x => x.Questions).FirstOrDefault(y => y.Id == id).Questions.ToList();
            Assert.NotNull(quests);
            Assert.True(quests.Count() != 0);
            Assert.True(quests.ElementAt(0).Suggested_Openings.Count() == 0);

            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }


        //Test Case ID: HWS12
        [Test]
        public void testUpdateHomeworkNonExistant()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();

            Assert.Null(serv.GetByDetails("98", hwDesc));


            entity.Title = "98";


            // Act
            MichtavaResult res = serv.Update(new Homework("98", hwDesc,DateTime.Now, ctx.Set<Teacher>().FirstOrDefault(), ctx.Set<Text>().FirstOrDefault()));

            // Assert
            Assert.True(res is MichtavaFailure);
        }


        //Test Case ID: HWS13
        [Test]
        public void testUpdateHomeworkExistingDetails()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();
            Homework c = serv.All().FirstOrDefault();

            serv.Add(entity);

            Assert.AreEqual(count + 1, serv.All().Count());
            entity.Title = c.Title;
            entity.Description = c.Description;


            // Act
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaFailure);
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }


        //Test Case ID: HWS14
        [Test]
        public void testUpdateHomeworkNullText()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();
            Homework c = serv.All().FirstOrDefault();

            serv.Add(entity);

            Assert.AreEqual(count + 1, serv.All().Count());

            entity.Text = null; 

            // Act
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaFailure);
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }

        //Test Case ID: HWS15
        [Test]
        public void testUpdateHomeworkNullTeacher()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();
            Homework c = serv.All().FirstOrDefault();

            serv.Add(entity);

            Assert.AreEqual(count + 1, serv.All().Count());

            entity.Created_By = null;

            // Act
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaFailure);
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }

        //Deletes

        //Test Case ID: HWS16
        [Test]
        public void testDeleteHomeworkSuccess()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            serv.Add(entity);
            int count = serv.All().Count();

            Guid id = serv.GetByDetails(hwTitl, hwDesc).Id;

            // Act
            MichtavaResult res = serv.Delete(entity);


            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
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


        //Test Case ID: HWS17
        [Test]
        public void testDeleteNonExistant()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            int count = serv.All().Count();

            entity.setId(Guid.NewGuid());
            Assert.Null(serv.GetById(entity.Id));

            // Act
            MichtavaResult res = serv.Delete(entity);


            // Assert
            Assert.True(res is MichtavaFailure);

        }



        //Test Case ID: HWS18
        [Test]
        public void testHardDeleteSuccessHomework()
        {
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            // Arrange
            serv.Add(entity);
            int count = serv.All().Count();

            Guid id = serv.GetByDetails(hwTitl, hwDesc).Id;

            // Act
            MichtavaResult res = serv.HardDelete(entity);


            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.Null(serv.GetByDetails(hwTitl, hwDesc));
            Assert.True(serv.All().Count() == count - 1);
        }

        //Customs


      


        //add question, remove question, quickly tho

    }
}
