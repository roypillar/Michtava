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
    class TextServiceTests
    {
        private ApplicationDbContext ctx;

        private TextService serv;

        private Text entity;

        private const string n = "טסטינגטון";



        [OneTimeSetUp]
        public void oneTimeSetUp()
        {
            var connection = DbConnectionFactory.CreateTransient();
            this.ctx = new ApplicationDbContext(connection);
            this.serv = new TextService(new TextRepository(ctx));
            new DatabaseSeeder().CreateDependenciesAndSeed(ctx);//heavy duty

        }

        [SetUp]
        public void setUp()
        {
            entity = new Text(n,this.ctx.Set<Subject>().FirstOrDefault());
        }

        //Adds


        //Test case ID : Txs1
        [Test]
        public void testAddTextStandalone()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();


            // Act
            MichtavaResult res = serv.Add(entity);

            // Assert
            Assert.AreEqual(count + 1, serv.All().Count());
            Assert.NotNull(serv.GetByName(n));
            Assert.True(res is MichtavaSuccess);
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }


        //Test case ID : Txs2
        [Test]
        public void testAddTextExistingId()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();
            Text existing = serv.All().FirstOrDefault();

            // Act
            MichtavaResult res = serv.Add(existing);

            // Assert
            Assert.True(res is MichtavaFailure);
        }

        
        //Test case ID : Txs3
        [Test]
        public void testAddTextExistingDetails()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();
            Text existing = serv.All().FirstOrDefault();

            Text c = new Text(existing.Name, this.ctx.Set<Subject>().FirstOrDefault());
            // Act

            MichtavaResult res = serv.Add(c);

            // Assert
            Assert.True(res is MichtavaFailure);

        }


        //Gets

        //Test case ID : Txs4
        [Test]
        public void testGetTextByIdTrue()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();

            Text c = serv.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Text actual = serv.GetById(c.Id);
            // Assert

            Assert.NotNull(actual);

        }


        //Test case ID : Txs5
        [Test]
        public void testGetTextByIdFalse()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();

            // Act
            Text actual = serv.GetById(Guid.NewGuid());
            // Assert

            Assert.Null(actual);

        }


        //Test case ID : Txs6

        [Test]
        public void testGetTextByDetailsTrue()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();

            Text c = serv.All().FirstOrDefault();
            Assert.NotNull(c);


            // Act
            Text actual = serv.GetByName(c.Name);
            // Assert

            Assert.NotNull(actual);
        }



        //Test case ID : Txs7
        [Test]
        public void testGetTextByDetailsFalse()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();


            // Act
            Text actual = serv.GetByName("14");
            // Assert

            Assert.Null(actual);

        }

        //Update


        //Test case ID : Txs8
        [Test]
        public void testUpdateTextSuccess()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();
            serv.Add(entity);

            Assert.Null(serv.GetByName("harambe"));


            Assert.AreEqual(count + 1, serv.All().Count());
            entity.Name = "harambe";


            // Act
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.NotNull(serv.GetByName("harambe"));
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }


        //Test case ID : Txs9
        [Test]
        public void testUpdateTextNonExistant()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();

            Assert.Null(serv.GetByName("harambe"));


            entity.Name = "harambe";


            // Act
            MichtavaResult res = serv.Update(new Text("harambe", this.ctx.Set<Subject>().FirstOrDefault()));
            // Assert
            Assert.True(res is MichtavaFailure);
        }


        //Test case ID : Txs10
        [Test]
        public void testUpdateTextExistingDetails()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();
            Text c = serv.All().FirstOrDefault();

            serv.Add(entity);

            Assert.AreEqual(count + 1, serv.All().Count());
            entity.Name = c.Name;


            // Act
            MichtavaResult res = serv.Update(entity);

            // Assert
            Assert.True(res is MichtavaFailure);
            Assert.True(serv.HardDelete(entity) is MichtavaSuccess);
        }



        //Deletes


        //Test case ID : Txs11
        [Test]
        public void testDeleteTextSuccess()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            serv.Add(entity);
            int count = serv.All().Count();

            Guid id = serv.GetByName(n).Id;

            // Act
            MichtavaResult res = serv.Delete(entity);


            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.Null(serv.GetByName(n));
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


        //Test case ID : Txs12
        [Test]
        public void testDeleteTextNonExistant()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            int count = serv.All().Count();

            entity.setId(Guid.NewGuid());
            Assert.Null(serv.GetById(entity.Id));

            // Act
            MichtavaResult res = serv.Delete(entity);


            // Assert
            Assert.True(res is MichtavaFailure);

        }



        //Test case ID : Txs13
        [Test]
        public void testHardDeleteTextSuccess()
        {
            Assert.Null(serv.GetByName(n));
            // Arrange
            serv.Add(entity);
            int count = serv.All().Count();

            Guid id = serv.GetByName(n).Id;

            // Act
            MichtavaResult res = serv.HardDelete(entity);


            // Assert
            Assert.True(res is MichtavaSuccess);
            Assert.Null(serv.GetByName(n));
            Assert.True(serv.All().Count() == count - 1);
        }

        //Customs





        //add question, remove question, quickly tho

    }
}
