using Entities.Models;
using Frontend.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Frontend.DataContexts
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Text> Texts { get; set; }//all tables belong here
        //tables tables tables
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public List<Text> getTexts(string t)
        {
            return (
                    from text in Texts
                    where text.Name.Contains(t)
                    select text
                 ).ToList<Text>();
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Do not pluralize the db tables
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}

