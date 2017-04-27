namespace Dal.Migrations
{
    using Common;
    using Entities.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Dal.ApplicationDbContext> , IDisposable
    {

        //superadmin: U:superadmin P:111
        //admin: U:admin P:111



        private UserManager<ApplicationUser> userManager;

        private RoleManager<IdentityRole> roleManager;

        private const int HighestGrade = 12;

        private const int ClassStudentsNumber = 20;

        private const int GradeClassesNumber = 5;

        private readonly List<string> personNames = new List<string>()
        {
            "טדי פררה",
            "דיאן פישר",
            "אנה ברושניקוב",
            "מריה פיניגן",
            "רוני פולץ",
            "אלינור ריגבי",
            "יעקב מגריזו",
            "בובי קורנפילד",
            "קריסטינה אגואילרה",
            "אלכסנדר בן ינאי",
            "סימון בן סימון",
            "פיטר מקלאופלאן",
            "אשלי הונג",
            "היידן ג'אקס",
            "אידה יעקובסון",
            "ג'יימי מילר",
            "ג'ייסון פטרסון",
            "מיכאל קאיסר",
            "אייבי קירני",
            "סמי מגריזו",
        };

        private readonly List<string> SubjectNames = new List<string>()
        {
            "ספרות",
            "לשון",
            "מתמטיקה",
            "מדעי המחשב",
            "אומנות",
            "מוזיקה ים תיכונית",
            "ספורט"
        };


        private readonly DateTime startDate = new DateTime(2016, 3, 22);

        private readonly DateTime endDate = new DateTime(2017, 3, 22);

        //private int studentCounter = 1;

        //private int teacherCounter = 1;


        private bool disposed = false;
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

    

        protected override void Seed(Dal.ApplicationDbContext context)
        {
            this.userManager = this.CreateUserManager(context);
            this.roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            context.Configuration.AutoDetectChangesEnabled = false;

            this.SeedRoles(context);
            this.SeedAdministrators(context);
            this.SeedOtherUsers(context);
            //this.SeedAcademicYears(context, AcademicYearsCount);

            this.SeedSubjects(context);

            context.Configuration.AutoDetectChangesEnabled = true;
           
        }

        private void SeedOtherUsers(ApplicationDbContext context)
        {
            this.SeedStudents(context);
            this.SeedTeachers(context);

        }

        private void SeedSubjects(ApplicationDbContext context)
        {
            if (context.Subjects.Any())
                return;

            foreach (var subjectName in SubjectNames)
            {
                Subject subject = new Subject();
                subject.Name = subjectName;
                subject.TotalHours = 80;
                context.Subjects.AddOrUpdate(subject);
            }
        }

        private void SeedRoles(ApplicationDbContext context)
        {
            if (context.Roles.Any())
            {
                return;
            }

            context.Roles.AddOrUpdate(new IdentityRole(GlobalConstants.SuperAdministratorRoleName));
            context.Roles.AddOrUpdate(new IdentityRole(GlobalConstants.AdministratorRoleName));
            context.Roles.AddOrUpdate(new IdentityRole(GlobalConstants.TeacherRoleName));
            context.Roles.AddOrUpdate(new IdentityRole(GlobalConstants.StudentRoleName));

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
            if (!this.roleManager.RoleExists(GlobalConstants.SuperAdministratorRoleName))
            {
                this.roleManager.Create(new IdentityRole(GlobalConstants.SuperAdministratorRoleName));
            }

            if (!this.roleManager.RoleExists(GlobalConstants.AdministratorRoleName))
            {
                this.roleManager.Create(new IdentityRole(GlobalConstants.AdministratorRoleName));
            }

            var result = this.userManager.Create(adminUser, password);

            if (result.Succeeded)
            {
                this.userManager.AddToRole(adminUser.Id, GlobalConstants.AdministratorRoleName);

                if (adminUser.UserName == "superadmin")
                {
                    this.userManager.AddToRole(adminUser.Id, GlobalConstants.SuperAdministratorRoleName);
                }
            }
        }

        private void SeedStudents(ApplicationDbContext context)
        {

            if (context.Students.Any())
            {
                return;
            }

            var prof = new Student()
            {
                Name = "Student Studentson"
            };

            var user = new ApplicationUser()
            {
                UserName = "student111",
                Email = "student@gmail.com",
                PhoneNumber = "1111111111"
                
            };

            const string Password = "111";

            this.SeedStudentApplicationUser(user, Password);

            prof.ApplicationUser = user;

            context.Students.Add(prof);

            context.SaveChanges();
        }

        private void SeedTeachers(ApplicationDbContext context)
        {

            if (context.Teachers.Any())
            {
                return;
            }

            var prof = new Teacher()
            {
                Name = "Teacher Teacherson"
            };

            var user = new ApplicationUser()
            {
                UserName = "teacher111",
                Email = "teacher@gmail.com",
                PhoneNumber = "1111111111"

            };

            const string Password = "111";

            this.SeedTeacherApplicationUser(user, Password);

            prof.ApplicationUser = user;

            context.Teachers.Add(prof);

            context.SaveChanges();
        }

        private void SeedStudentApplicationUser(ApplicationUser user, string password)
        {
            if (!this.roleManager.RoleExists(GlobalConstants.StudentRoleName))
            {
                this.roleManager.Create(new IdentityRole(GlobalConstants.StudentRoleName));
            }

          

            var result = this.userManager.Create(user, password);

            if (result.Succeeded)
            {
                this.userManager.AddToRole(user.Id, GlobalConstants.StudentRoleName);

        
            }
        }

        private void SeedTeacherApplicationUser(ApplicationUser user, string password)
        {
            if (!this.roleManager.RoleExists(GlobalConstants.TeacherRoleName))
            {
                this.roleManager.Create(new IdentityRole(GlobalConstants.TeacherRoleName));
            }



            var result = this.userManager.Create(user, password);

            if (result.Succeeded)
            {
                this.userManager.AddToRole(user.Id, GlobalConstants.TeacherRoleName);


            }
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

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);

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
    }
}
