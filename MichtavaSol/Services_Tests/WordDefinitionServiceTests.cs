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
    class WordDefinitionServiceTests
    {
        private ApplicationDbContext ctx;

        private WordDefinitionService serv;

        private WordDefinition entity;

        private const string word = "טסטינגטון";

        private const string def = "חציליטי";



        [OneTimeSetUp]
        public void oneTimeSetUp()
        {
            var connection = DbConnectionFactory.CreateTransient();
            this.ctx = new ApplicationDbContext(connection);
            this.serv = new WordDefinitionService(new WordDefinitionRepository(ctx));
            new DatabaseSeeder().CreateDependenciesAndSeed(ctx);//heavy duty

        }

        [SetUp]
        public void setUp()
        {
            entity = new WordDefinition(word,def);
        }

        //Adds
        [Test]
        public void testAddWordDefinitionStandalone()
        {
            Assert.Null(serv.GetByWord(word));
            // Arrange
            int count = serv.All().Count();


            // Act
            MichtavaResult res = serv.Add(entity);

            // Assert
            Assert.AreEqual(count + 1, serv.All().Count());
            Assert.NotNull(serv.GetByWord(word));
            Assert.True(res is MichtavaSuccess);
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }

        [Test]
        public void testAddExistingIdWordDefinition()
        {
            Assert.Null(serv.GetByWord(word));
            // Arrange
            int count = serv.All().Count();
            WordDefinition existing = serv.All().FirstOrDefault();

            // Act
            MichtavaResult res = serv.Add(existing);

            // Assert
            Assert.True(res is MichtavaFailure);
        }

        [Test]
        public void testAddExistingWordDefinitionDetails()
        {
            Assert.Null(serv.GetByWord(word));
            // Arrange
            int count = serv.All().Count();
            WordDefinition existing = serv.All().FirstOrDefault();

            WordDefinition c = new WordDefinition(existing.Word,"some definition");
            // Act

            MichtavaResult res = serv.Add(c);

            // Assert
            Assert.True(res is MichtavaFailure);

        }


        [Test]
        public void testAddWithTextInconsistency()
        {
            Assert.Null(serv.GetByWord(word));
        }


        [Test]
        public void testAddWordDefinitionWithTeacherInconsistency()
        {
            Assert.Null(serv.GetByWord(word));
        }

        //Gets
        [Test]
        public void testGetWordDefinitionByIdTrue()
        {
            Assert.Null(serv.GetByWord(word));
            // Arrange
            int count = serv.All().Count();

            WordDefinition c = serv.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            WordDefinition actual = serv.GetByWord(c.Word);
            // Assert

            Assert.NotNull(actual);

        }

        [Test]
        public void testGetWordDefinitionByIdFalse()
        {
            Assert.Null(serv.GetByWord(word));
            // Arrange
            int count = serv.All().Count();

            // Act
            WordDefinition actual = serv.GetByWord(Guid.NewGuid().ToString());
            // Assert

            Assert.Null(actual);

        }


        [Test]
        public void testGetWordDefinitionByDetailsTrue()
        {
            Assert.Null(serv.GetByWord(word));
            // Arrange
            int count = serv.All().Count();

            WordDefinition c = serv.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            WordDefinition actual = serv.GetByWord(c.Word);
            // Assert

            Assert.NotNull(actual);
        }



        [Test]
        public void testGetWordDefinitionByDetailsFalse()
        {
            Assert.Null(serv.GetByWord(word));
            // Arrange
            int count = serv.All().Count();


            // Act
            WordDefinition actual = serv.GetByWord("14");
            // Assert

            Assert.Null(actual);

        }

        //Update
        [Test]
        public void testUpdateWordDefinitionSuccess()
        {
            Assert.Null(serv.GetByWord(word));
            // Arrange
            int count = serv.All().Count();
            serv.Add(entity);

            Assert.Null(serv.GetByWord("harambe"));


            Assert.AreEqual(count + 1, serv.All().Count());
            entity.addDefinition ("harambererer is good      ");


            // Act
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.NotNull(serv.GetByWord(word));
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }

        [Test]
        public void testUpdateWordDefinitionNonExistant()
        {
            Assert.Null(serv.GetByWord(word));
            // Arrange
            int count = serv.All().Count();

            Assert.Null(serv.GetByWord("harambe"));

            // Act
            MichtavaResult res = serv.Update(new WordDefinition("harambe","some definition"));
            // Assert
            Assert.True(res is MichtavaFailure);
        }

        //[Test]
        //public void testUpdateWordDefinitionExistingDetails()
        //{
        //    Assert.Null(serv.GetByWord(word));
        //    // Arrange
        //    int count = serv.All().Count();
        //    WordDefinition c = serv.All().FirstOrDefault();

        //    serv.Add(entity);

        //    Assert.AreEqual(count + 1, serv.All().Count());
        //    entity.Word = c.Word;

        //    //Assert
        //    Assert.Throws<InvalidOperationException>(() =>  serv.Update(entity));

        //    Assert.True(serv.HardDelete(entity) is MichtavaSuccess);

        //}



        //Deletes
        [Test]
        public void testDeleteWordDefinitionSuccess()
        {
            Assert.Null(serv.GetByWord(word));
            // Arrange
            serv.Add(entity);
            int count = serv.All().Count();

            string id = serv.GetByWord(word).Word;

            // Act
            MichtavaResult res = serv.Delete(entity);


            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.Null(serv.GetByWord(word));
            Assert.True(serv.All().Count() == count - 1);
            Assert.Null(serv.GetByWord(id));

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

        [Test]
        public void testDeleteWordDefinitionNonExistant()
        {
            Assert.Null(serv.GetByWord(word));
            // Arrange
            int count = serv.All().Count();

            entity.setId(Guid.NewGuid());
            Assert.Null(serv.GetByWord(entity.Word));

            // Act
            MichtavaResult res = serv.Delete(entity);


            // Assert
            Assert.True(res is MichtavaFailure);

        }



        [Test]
        public void testHardDeleteWordDefinitionSuccess()
        {
            Assert.Null(serv.GetByWord(word));
            // Arrange
            serv.Add(entity);
            int count = serv.All().Count();


            // Act
            MichtavaResult res = serv.HardDelete(entity);


            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.Null(serv.GetByWord(word));
            Assert.True(serv.All().Count() == count - 1);
        }

        //Customs





        //add question, remove question, quickly tho

    }
}
