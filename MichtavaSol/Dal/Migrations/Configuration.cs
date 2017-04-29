namespace Dal.Migrations
{
    using Common;
    using Entities.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Repositories;
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

        private readonly List<string> studentNames = new List<string>()//11
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
            "סימון בן סימון" };

        private readonly List<string> studentUserNames = new List<string>()//11
        {
            "teddy125",
            "fishbomb_forever",
            "callmeanna",
            "immaculate_conception997",
            "fiverfourfivee",
            "roygbivbabyy33",
            "יעקב מגריזו",
            "conr5",
            "xtina662",
            "rara",
            "סימון בן סימון" };

        private readonly List<string> teacherNames = new List<string>() { //9
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

        private readonly List<string> teacherUserNames = new List<string>() {//9
            "peteFastBoy555",
            "hongster",
            "mcHeyHey",
            "IdrisElba2020",
            "yeahItsOkay",
            "camelOrNext",
            "classact123",
            "אייבי קירני",
            "סמי מגריזו",
        };

        private readonly List<string> SubjectNames = new List<string>()//7
        {
            "ספרות",
            "לשון",
            "מתמטיקה",
            "מדעי המחשב",
            "אומנות",
            "מוזיקה ים תיכונית",
            "ספורט"
        };

        private readonly List<KeyValuePair<string, int>> SchoolClasses = new List<KeyValuePair<string, int>>()//3
        {
            new KeyValuePair<string, int>("ג",3),
            new KeyValuePair<string, int>("ז",8),
            new KeyValuePair<string, int>("ח",18),
        };

        private const string PASSWORD = "111";

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
            //SchoolClassRepository scRepo = new SchoolClassRepository(context);



            this.userManager = this.CreateUserManager(context);
            this.roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            context.Configuration.AutoDetectChangesEnabled = false;

            this.SeedRoles(context);
            this.SeedSubjects(context);
            this.SeedAdministrators(context);


            this.SeedStudents(context);
            this.SeedTeachers(context);
            this.SeedSchoolClasses(context);
            //this.SeedAcademicYears(context, AcademicYearsCount);


            context.Configuration.AutoDetectChangesEnabled = true;
           
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
            context.SaveChanges();
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



            this.SeedAdminApplicationUser(adminUser, PASSWORD);

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

            this.SeedAdminApplicationUser(adminUser, PASSWORD);

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


            
            for(int i = 0; i < studentNames.Count; i++)
            {
                string name = studentNames.ElementAt(i);
                string userName = studentUserNames.ElementAt(i);

                 Student prof = new Student()
                {
                    Name = name
                };

                ApplicationUser user = new ApplicationUser()
                {
                    UserName = userName,
                    Email = userName + "@gmail.com",
                    PhoneNumber = RandomDigits(10)

                };

                this.SeedStudentApplicationUser(user, PASSWORD);

                prof.ApplicationUser = user;

                context.Students.Add(prof);

                context.SaveChanges();

            }





           
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

        public string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

        private void SeedTeachers(ApplicationDbContext context)
        {

            if (context.Teachers.Any())
            {
                return;
            }

            for (int i = 0; i < teacherNames.Count; i++)
            {
                string name = teacherNames.ElementAt(i);
                string userName = teacherUserNames.ElementAt(i);

                Teacher prof = new Teacher()
                {
                    Name = name
                };

                ApplicationUser user = new ApplicationUser()
                {
                    UserName = userName,
                    Email = userName + "@gmail.com",
                    PhoneNumber = RandomDigits(10)

                };

                this.SeedTeacherApplicationUser(user, PASSWORD);

                prof.ApplicationUser = user;

                context.Teachers.Add(prof);

                context.SaveChanges();

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


        private void SeedSchoolClasses(ApplicationDbContext context)
        {

         

            IQueryable<Student> rtn = from temp in context.Students select temp;
            var students = new Queue<Student>(rtn.ToList());

            IQueryable<Teacher> rtn2 = from temp in context.Teachers select temp;
            var teachers = new Queue<Teacher>( rtn2.ToList());

            const  int studentsPerClass = 3;
            const  int teachersPerClass = 3;
            foreach (KeyValuePair<string,int> pair in SchoolClasses) {

                SchoolClass sc = new SchoolClass() { ClassLetter=pair.Key, GradeYear = pair.Value };
               


                foreach (Subject s in context.Subjects)
                {
                    sc.Subjects.Add(s);
                }

                context.SchoolClasses.AddOrUpdate(sc);

                //if (System.Diagnostics.Debugger.IsAttached == false)
                //{

                //    System.Diagnostics.Debugger.Launch();

                //}

                SchoolClass scPersistant = context.SchoolClasses.Where(x => (x.GradeYear==sc.GradeYear && x.ClassLetter==sc.ClassLetter)).FirstOrDefault();

                for (int i = 0; i < studentsPerClass; i++) {
                    Student s = students.Dequeue();
                    scPersistant.Students.Add(s);
                    s.SchoolClass = scPersistant;
                    context.Students.AddOrUpdate(s);
                }

                for (int i = 0; i < teachersPerClass; i++)
                {
                    Teacher t = teachers.Dequeue();
                    scPersistant.Teachers.Add(t);
                    t.SchoolClasses.Add(scPersistant);
                    context.Teachers.AddOrUpdate(t);
                }


                context.SchoolClasses.AddOrUpdate(sc);



                context.SaveChanges();

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
