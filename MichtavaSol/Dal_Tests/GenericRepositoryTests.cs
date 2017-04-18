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

namespace Dal_Tests
{
    [TestFixture]
    class GenericRepositoryTests
    {
        private IApplicationDbContext ctx;


        [SetUp]
        void setUp() { 
        
            this.ctx = ApplicationDbContext.Create();
        }

        void testWithAnswers()
        {
            IDbSet<Answer> set = ctx.Set<Answer>();
        }
    }
    
}
