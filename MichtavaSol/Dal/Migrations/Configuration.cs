using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Common;
using Entities.Models;


namespace Dal.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>, IDisposable
    {
        private const int HighestGrade = 12;

        private const int ClassStudentsNumber = 20;

        private const int GradeClassesNumber = 5;

        private const string DefaultProfileImageUrl =
            "/Content/Images/default-profile-image.png";

        private readonly List<string> personNames = new List<string>()
        {
            "שלום חנוך",
             "מאיר בנאי",
              "Ricky Gal",
               "עדי רן",
                "מנחם אבוטבול",
                 "יואב אורלין",
        };

        private readonly List<string> generalSchoolThemeSubjectNames = new List<string>()
        {
            "ספרות",
            "לשון",
            "חשבון",
            "אומנות",
            "מחשבת ישראל",
            "ספורט"
        };


        private readonly DateTime startDate = new DateTime(2012, 9, 15);

        private readonly DateTime endDate = new DateTime(2013, 5, 31);

        private int studentCounter = 1;

        private int teacherCounter = 1;

        private UserManager<ApplicationUser> userManager;

        private RoleManager<IdentityRole> roleManager;

        private bool disposed = false;

        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Seed(ApplicationDbContext context)
        {
            this.userManager = this.CreateUserManager(context);
            this.roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            context.Configuration.AutoDetectChangesEnabled = false;

            this.SeedRoles(context);
            this.SeedAdministrators(context);
            //add all the seeds we want

            context.Configuration.AutoDetectChangesEnabled = true;
        }

        private UserManager<ApplicationUser> CreateUserManager(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Configure user manager
            // Configure validation logic for usernames
            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 3,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            return userManager;
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.roleManager.Dispose();
                }
            }

            this.disposed = true;
        }

        private void SeedRoles(ApplicationDbContext context)
        {
            if (context.Roles.Any())
            {
                return;
            }

            context.Roles.AddOrUpdate(new IdentityRole(Common.Constants.SuperAdministratorRoleName));
            context.Roles.AddOrUpdate(new IdentityRole(Common.Constants.AdministratorRoleName));
            context.Roles.AddOrUpdate(new IdentityRole(Common.Constants.TeacherRoleName));
            context.Roles.AddOrUpdate(new IdentityRole(Common.Constants.StudentRoleName));

            context.SaveChanges();
        }

        private void SeedAdministrators(ApplicationDbContext context)
        {
            if (context.Administrators.Any())
            {
                return;
            }

            var adminProfile = new Administrator()
            {
                FirstName = "SuperAdmin",
                LastName = "SuperAdmin"
            };

            var adminUser = new ApplicationUser()
            {
                UserName = "superadmin",
                Email = "superadmin@superadmin.com"
            };

            const string Password = "111";

            this.SeedAdminApplicationUser(adminUser, Password);

            adminProfile.ApplicationUser = adminUser;

            context.Administrators.Add(adminProfile);

            adminProfile = new Administrator()
            {
                FirstName = "Admin",
                LastName = "Admin"
            };

            adminUser = new ApplicationUser()
            {
                UserName = "admin",
                Email = "admin@admin.com"
            };

            this.SeedAdminApplicationUser(adminUser, Password);

            adminProfile.ApplicationUser = adminUser;

            context.Administrators.Add(adminProfile);

            context.SaveChanges();
        }

        private void SeedAdminApplicationUser(ApplicationUser adminUser, string password)
        {
            if (!this.roleManager.RoleExists(Common.Constants.SuperAdministratorRoleName))
            {
                this.roleManager.Create(new IdentityRole(Common.Constants.SuperAdministratorRoleName));
            }

            if (!this.roleManager.RoleExists(Common.Constants.AdministratorRoleName))
            {
                this.roleManager.Create(new IdentityRole(Common.Constants.AdministratorRoleName));
            }

            var result = this.userManager.Create(adminUser, password);

            if (result.Succeeded)
            {
                this.userManager.AddToRole(adminUser.Id, Common.Constants.AdministratorRoleName);

                if (adminUser.UserName == "superadmin")
                {
                    this.userManager.AddToRole(adminUser.Id, Common.Constants.SuperAdministratorRoleName);
                }
            }
        }



        

          

          
      

        

          private static List<SchoolClass> CopyClassesFromPreviousYearCurrentGrade(
              ApplicationDbContext context,
              Grade grade,
              IList<SchoolClass> previousYearCurrentGradeClasses)
          {
              List<SchoolClass> schoolClasses = new List<SchoolClass>();

              foreach (var previousYearCurrentGradeClass in previousYearCurrentGradeClasses)
              {
                  SchoolClass schoolClass = new SchoolClass();
                  schoolClass.Grade = grade;
                  schoolClass.ClassLetter = previousYearCurrentGradeClass.ClassLetter;

                  context.SchoolClasses.AddOrUpdate(schoolClass);
                  schoolClasses.Add(schoolClass);
              }

              return schoolClasses;
          }

          private List<SchoolClass> SeedGradeSchoolClasses(
              ApplicationDbContext context, 
              Grade grade,
              IList<SchoolClass> previousYearCurrentGradeClasses,
              int gradeClassesNumber)
          {
              List<SchoolClass> schoolClasses = new List<SchoolClass>();

              if (previousYearCurrentGradeClasses != null && previousYearCurrentGradeClasses.Any())
              {
                  schoolClasses = 
                      CopyClassesFromPreviousYearCurrentGrade(context, grade, previousYearCurrentGradeClasses);
              }
              else
              {
                  schoolClasses = this.CreateGradeNewSchoolClasses(context, grade, gradeClassesNumber);
              }

              return schoolClasses;
          }

          private List<SchoolClass> CreateGradeNewSchoolClasses(
              ApplicationDbContext context, 
              Grade grade, 
              int gradeClassesNumber)
          {
              List<SchoolClass> schoolClasses = new List<SchoolClass>();
              int charANumber = 'A';

              for (int currentChar = charANumber; currentChar < charANumber + gradeClassesNumber; currentChar++)
              {
                  SchoolClass schoolClass = new SchoolClass();
                  schoolClass.Grade = grade;
                  schoolClass.ClassLetter = ((char)currentChar).ToString();

                 
                  
                  context.SchoolClasses.AddOrUpdate(schoolClass);
                  schoolClasses.Add(schoolClass);
              }

              return schoolClasses;
          }

          private List<Student> SeedSchoolClassStudents(
              ApplicationDbContext context,
              SchoolClass oldSchoolClass,
              int classStudentsNumber)
          {
              var students = new List<Student>();

              if (oldSchoolClass != null && oldSchoolClass.Students.Any())
              {
                  students = oldSchoolClass.Students;
              }
              else
              {
                  students = this.CreateClassOfStudents(context, classStudentsNumber);
              }

              return students;
          }

          private List<Student> CreateClassOfStudents(
              ApplicationDbContext context,
              int classStudentsNumber)
          {
              var students = new List<Student>();
              for (int i = 0; i < classStudentsNumber; i++)
              {
                  var student = this.CreateSingleStudent();
                  context.Students.Add(student);
                  students.Add(student);
              }

              return students;
          }

          private Student CreateSingleStudent()
          {
              var studentProfile = new Student();
              Random rand = new Random();
              studentProfile.Name = this.personNames[rand.Next(0, this.personNames.Count() - 1)];

              // Create Student Role if it does not exist
              if (!this.roleManager.RoleExists(Common.Constants.StudentRoleName))
              {
                  this.roleManager.Create(new IdentityRole(Common.Constants.StudentRoleName));
              }

              // Create Student User with password
              var studentUser = new ApplicationUser()
              {
                  UserName = "student" + this.studentCounter.ToString("D4"),
                  Email = "s" + this.studentCounter.ToString("D4") + "@s.com",
                  ImageUrl = DefaultProfileImageUrl
              };

              this.studentCounter++;

              const string Password = "111";

              var result = this.userManager.Create(studentUser, Password);

              // Add Student User to Student Role
              if (result.Succeeded)
              {
                  this.userManager.AddToRole(studentUser.Id, Common.Constants.StudentRoleName);
              }

              // Add Student User to Student Profile
              studentProfile.ApplicationUser = studentUser;

              return studentProfile;
          }

          private List<Teacher> SeedTeachers(
              ApplicationDbContext context, 
              IList<Grade> currentAcademicYearGrades, 
              IList<Grade> previousAcademicYearGrades)
          {
              var teachers = new List<Teacher>();

              foreach (var grade in currentAcademicYearGrades)
              {
                  foreach (var subject in grade.Subjects)
                  {
                      var previousAcademicYearGradeSubject = new Subject();
                      var teacherProfile = new Teacher();

                      Grade previousAcademicYearCurrentGrade = previousAcademicYearGrades
                          .FirstOrDefault(g => g.GradeYear == grade.GradeYear);

                      if (previousAcademicYearGrades.Any() && previousAcademicYearCurrentGrade != null)
                      {
                          previousAcademicYearGradeSubject = previousAcademicYearCurrentGrade
                              .Subjects
                              .FirstOrDefault(s => s.Name == subject.Name);

                          if (previousAcademicYearGradeSubject != null)
                          {
                              context.Subjects.AddOrUpdate(subject);
                          }
                          else
                          {
                              teacherProfile = this.CreateSingleTeacher();
                              context.Teachers.Add(teacherProfile);
                              teachers.Add(teacherProfile);
                          }
                      }
                      else
                      {
                          teacherProfile = this.CreateSingleTeacher();
                          context.Teachers.Add(teacherProfile);
                          teachers.Add(teacherProfile);
                      }
                  }
              }

              return teachers;
          }

          private Teacher CreateSingleTeacher()
          {
              var teacherProfile = new Teacher();
              var rand = new Random();
              teacherProfile.Name = this.personNames[rand.Next(0, this.personNames.Count())];

              // Create Teacher Role if it does not exist
              if (!this.roleManager.RoleExists(Common.Constants.TeacherRoleName))
              {
                  this.roleManager.Create(new IdentityRole(Common.Constants.TeacherRoleName));
              }

              var counter = this.teacherCounter.ToString("D3");

              // Create Teacher User with password
              var teacherUser = new ApplicationUser()
              {
                  UserName = "teacher" + counter,
                  Email = "t" + counter + "@t.com",
                  ImageUrl = DefaultProfileImageUrl
              };

              const string Password = "111";

              this.teacherCounter++;

              var result = this.userManager.Create(teacherUser, Password);

              // Add Teacher User to Teacher Role
              if (result.Succeeded)
              {
                  this.userManager.AddToRole(teacherUser.Id, Common.Constants.TeacherRoleName);
              }

              // Add Teacher User to Teacher Profile
              teacherProfile.ApplicationUser = teacherUser;

              return teacherProfile;
          }


      }
  }
 
    


