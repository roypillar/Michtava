using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Domain.Models;

namespace DAL
{
    public class UserDal : DbContext    //inheret dbContext to use entityFramework
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder) //this connect between the db tables
                                                                             //and the models we got **if they dont
                                                                             //have the same name..
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("tblUsers");         //here we connect them..
        }
        public DbSet<User> Users { get; set; }

        public List<User> doSomething(string searchValue)
        {
           return (
                   from user in Users
                   where user.FirstName.Contains(searchValue)
                   select user
                ).ToList<User>();
        }

    }
}