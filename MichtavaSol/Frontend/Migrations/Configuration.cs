namespace Frontend.Migrations
{
    using Entities.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Frontend.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Frontend.Models.ApplicationDbContext";
        }

        protected override void Seed(Frontend.Models.ApplicationDbContext context)
        {

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //Seed initial users doesn't work
            //var store = new UserStore<ApplicationUser>(context);
            //var manager = context.getU
            //var seededUser = new ApplicationUser { UserName = "seeded111" };

            //manager.CreateAsync(seededUser, "password");//currently does not work



            //Seed initial models
            //context.Texts.AddOrUpdate(
            //   t => t.Id,
            //   new Text
            //   {
            //       Id = Guid.NewGuid().ToString()
            //   });

        }
    }
}
