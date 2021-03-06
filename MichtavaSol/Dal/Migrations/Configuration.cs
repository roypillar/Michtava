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

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext> //IDisposable
    {

        //superadmin: U:superadmin P:111
        //admin: U:admin P:111



        private UserManager<ApplicationUser> userManager;

        private RoleManager<IdentityRole> roleManager;


        //        private const int HighestGrade = 12;

        //        private const int ClassStudentsNumber = 20;

        //        private const int GradeClassesNumber = 5;

        //        private readonly List<string> studentNames = new List<string>()//11
        //        {
        //            "��� ����",
        //            "���� ����",
        //            "��� ���������",
        //            "���� ������",
        //            "���� ����",
        //            "������ �����",
        //            "���� ������",
        //            "���� ��������",
        //            "�������� ��������",
        //            "������� �� ����",
        //            "����� �� �����" };

        //        private readonly List<string> studentUserNames = new List<string>()//11
        //        {
        //            "teddy125",
        //            "fishbomb_forever",
        //            "callmeanna",
        //            "immaculate_conception997",
        //            "fiverfourfivee",
        //            "roygbivbabyy33",
        //            "���� ������",
        //            "conr5",
        //            "xtina662",
        //            "rara",
        //            "����� �� �����" };

        //        private readonly List<string> teacherNames = new List<string>() { //9
        //            "���� ���������",
        //            "���� ����",
        //            "����� �'���",
        //            "���� ��������",
        //            "�'���� ����",
        //            "�'����� ������",
        //            "����� �����",
        //            "����� �����",
        //            "��� ������",
        //        };

        //        private readonly List<string> teacherUserNames = new List<string>() {//9
        //            "peteFastBoy555",
        //            "hongster",
        //            "mcHeyHey",
        //            "IdrisElba2020",
        //            "yeahItsOkay",
        //            "camelOrNext",
        //            "classact123",
        //            "����� �����",
        //            "��� ������",
        //        };

        //        private readonly List<string> SubjectNames = new List<string>()//7
        //        {
        //            "�����",
        //            "����",
        //            "�������",
        //            "���� �����",
        //            "������",
        //            "������ �� �������",
        //            "�����"
        //        };

        //        private readonly List<KeyValuePair<string, int>> SchoolClasses = new List<KeyValuePair<string, int>>()//3
        //        {
        //            new KeyValuePair<string, int>("�",3),
        //            new KeyValuePair<string, int>("�",8),
        //            new KeyValuePair<string, int>("�",18),
        //        };

        //        private static readonly string text1 =

        //        "��� ������ ������, ������ ��� ���� ��� ������ ���� ��� �� ���, ������� ����� �������.���� ��� ����� ������ �� ������ ����� ���� ���� �������� �� ����.��� �� �� ����� ��� �� ��� ������ �������, ��� �� ��� ����� (��� ����� ���� 10 ����? ���� ������ ���)." + "\n\n" +

        //"������� ������� ����� �� ��� ����(���� ������), ��� �� ������ � �� �� ���� �� ���.�� ���� ������ ��� �������� ���� ����� ������� �� ��� ������� ���� ��� ����." + "\n" +

        //"���� �� �� ��� ������ �� ���� ������, �� ������ �� ���� �����." + "\n" +

        //"����� ���� ����� ������� ���� ������ ��� (��� ����� �� �����), ��� ����� ������ ���� ��� ��� ����." + "\n" +

        //"���� ���� �� ������� ��� �����! ������� ����� ���� �� ��� ���� ������ ����.�� ������, �� ���� ����� ���� ���� ��� �� ��� �����." + "\n" +

        //"���� ���� �������� ���� ���� ������ ����� ���� 8 ����." + "\n" +

        //"�� ������, �� ���� ����� ���� ���� ��� �� ��� �����.";

        //        private readonly List<string> textStrings = new List<string>(){
        //            text1,
        //            "��� ���� ������ ����� ��� ��� ��'��� ������ ����� �������",
        //            "���� ����� ���� ������? �� ���� ��� �������. ���� ���,\n\n "+"��� ����� ���� ���� �������� �������."
        //        };

        //        private readonly List<string> textNames = new List<string>()
        //        {
        //            "����� ������'� ������",
        //            "�� ����� ����?",
        //            "������ �� �������"
        //        };

        //        private readonly List<string> HWtitles = new List<string>(){
        //            text1,
        //            "����� ������: ���� ����",
        //            "����� ����: ������ ����� ������?",
        //            "����� �������� ������ ������ 3�"
        //        };

        //        private readonly List<string> HWdescs = new List<string>(){
        //            text1,
        //            "����� ������ ����� �� ���� �� ������ �� ���� 17. �� ��� ������ �� ������ �������� ����?",
        //            "��� �����, ���� �'����,���� ���� ����� ������� ���� �� ���� ���� ����� ����. ��� ����������� ��� ��� ���� ����������� ������ �����'��?",
        //            "��� �� ���� ����� ������� ����� ����� ���� ������ �' ���� ��� ����� 44444."
        //        };


        //        private static List<SuggestedOpening> suggested_openings = new List<SuggestedOpening>() {
        //            new SuggestedOpening("������� ������ �� ����:"),
        //             new SuggestedOpening("������ ����� ������ ��� ��������:"),
        //             new SuggestedOpening("this is a suggested opening in english"),
        //             new SuggestedOpening(" ������ �� ���� ����� ���� ������ �������� ������ �"),
        //             new SuggestedOpening("������ ����� ��� ������ ����:"),
        //              new SuggestedOpening("������� �� ������ �� ���� �� ����;"),
        //              new SuggestedOpening("������� �� ���� ���� ��: \n 1)��� �� ��")
        //        };




        //        private readonly List<Question> questions = new List<Question>()
        //        {
        //            new Question("���� ��� ������� �� ������ ����� ���� �����'�"),
        //            new Question("��� �� �� ������ ��� ���� ���� �� ���� ������:"),
        //            new Question("����� ����� �� ���� ����� �� ������, ��� �� ���� ____? (���� �� ����)"),
        //            new Question("����� ��� \"����\" �������� �� ����� ����, ���� ���� �� ��������?"),
        //            new Question("���� ��� ���� ����� ���� ������ ��� ������ �������?"),
        //            new Question("�� ��� ���� ������� (2017)?"),
        //            new Question("this is a question in english"),
        //            new Question("�� ���� ��� ����� ��� ������, ��� �����?"),
        //            new Question("�� ���� ��� ����� ��� ������, ��� �����?"),
        //            new Question("�� ��� ���� �� ��� ������� ���� �������� �����?"),

        //        };

        //        private readonly List<string> answerContents = new List<string>()
        //        {
        //            "������� �� ��� ������� ���, �� �� ���� �� �� ��",
        //           "���� ���� �� ������� �����: ���� ����������, ������ ��� ����� �����, ������ ����� ��� �����",
        //           "����� ������� ������ ������� �'��� ����� ����� ��� ������",
        //           "����� �� ��� ���� 1492 ��� ���� ���� ��� ��� ���� �� �� ��� �������� ������",
        //           "���� ���� ���� �� ���� ����� �� ��� ������ ���",
        //           "������ ����� ��\"� ��� �����, ���� ���� ����� ����",
        //           "this is an answer  in english",
        //           "������ ��� ����� ����� �����, ��� ��� ���� �� ���� ����� �����.",
        //            "������ ��� ����� ������� �����, ����� ������ �� ������ ���� ���� ����� 2 ������� ����� ��� ���",
        //          "����� �� �� ���� �������, ���� ���� ���� ����� "

        //        };




        //        private const string PASSWORD = "111";

        //        private readonly DateTime startDate = new DateTime(2016, 3, 22);

        //        private readonly DateTime endDate = new DateTime(2017, 3, 22);

        //private int studentCounter = 1;

        //private int teacherCounter = 1;


        private bool disposed = false;
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }



        protected override void Seed(ApplicationDbContext context)
        {

            this.userManager = this.CreateUserManager(context);
            this.roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            new DatabaseSeeder().Seed(context,userManager,roleManager);

        }

            //            this.SeedRoles(context);
            //            this.SeedSubjects(context);
            //            this.SeedAdministrators(context);


            //            this.SeedStudents(context);
            //            this.SeedTeachers(context);
            //            this.SeedSchoolClasses(context);
            //            this.SeedTexts(context);
            //            this.SeedHomeworks(context);
            //            this.SeedAnswers(context);
            //            //seeddefinitions?
            //            //fixseed

            //            context.Configuration.AutoDetectChangesEnabled = true;

            //        }



            //        private void SeedSubjects(ApplicationDbContext context)
            //        {
            //            if (context.Subjects.Any())
            //                return;

            //            foreach (var subjectName in SubjectNames)
            //            {
            //                Subject subject = new Subject();
            //                subject.Name = subjectName;
            //                subject.TotalHours = 80;
            //                context.Subjects.AddOrUpdate(subject);
            //            }
            //            context.SaveChanges();
            //        }

            //        private void SeedRoles(ApplicationDbContext context)
            //        {
            //            if (context.Roles.Any())
            //            {
            //                return;
            //            }

            //            context.Roles.AddOrUpdate(new IdentityRole(GlobalConstants.SuperAdministratorRoleName));
            //            context.Roles.AddOrUpdate(new IdentityRole(GlobalConstants.AdministratorRoleName));
            //            context.Roles.AddOrUpdate(new IdentityRole(GlobalConstants.TeacherRoleName));
            //            context.Roles.AddOrUpdate(new IdentityRole(GlobalConstants.StudentRoleName));

            //            context.SaveChanges();
            //        }

            //        private void SeedAdministrators(ApplicationDbContext context)
            //        {
            //            if (context.Administrators.Any())
            //            {
            //                return;
            //            }

            //            var adminProfile = new Administrator()
            //            {
            //                FirstName = "SuperAdmin",
            //                LastName = "SuperAdmin"
            //            };

            //            var adminUser = new ApplicationUser()
            //            {
            //                UserName = "superadmin",
            //                Email = "superadmin@superadmin.com"
            //            };



            //            this.SeedAdminApplicationUser(adminUser, PASSWORD);

            //            adminProfile.ApplicationUser = adminUser;

            //            context.Administrators.Add(adminProfile);

            //            adminProfile = new Administrator()
            //            {
            //                FirstName = "Admin",
            //                LastName = "Admin"
            //            };

            //            adminUser = new ApplicationUser()
            //            {
            //                UserName = "admin",
            //                Email = "admin@admin.com"
            //            };

            //            this.SeedAdminApplicationUser(adminUser, PASSWORD);

            //            adminProfile.ApplicationUser = adminUser;

            //            context.Administrators.Add(adminProfile);

            //            context.SaveChanges();
            //        }

            //        private void SeedAdminApplicationUser(ApplicationUser adminUser, string password)
            //        {
            //            if (!this.roleManager.RoleExists(GlobalConstants.SuperAdministratorRoleName))
            //            {
            //                this.roleManager.Create(new IdentityRole(GlobalConstants.SuperAdministratorRoleName));
            //            }

            //            if (!this.roleManager.RoleExists(GlobalConstants.AdministratorRoleName))
            //            {
            //                this.roleManager.Create(new IdentityRole(GlobalConstants.AdministratorRoleName));
            //            }

            //            var result = this.userManager.Create(adminUser, password);

            //            if (result.Succeeded)
            //            {
            //                this.userManager.AddToRole(adminUser.Id, GlobalConstants.AdministratorRoleName);

            //                if (adminUser.UserName == "superadmin")
            //                {
            //                    this.userManager.AddToRole(adminUser.Id, GlobalConstants.SuperAdministratorRoleName);
            //                }
            //            }
            //        }

            //        private void SeedStudents(ApplicationDbContext context)
            //        {

            //            if (context.Students.Any())
            //            {
            //                return;
            //            }



            //            for (int i = 0; i < studentNames.Count; i++)
            //            {
            //                string name = studentNames.ElementAt(i);
            //                string userName = studentUserNames.ElementAt(i);

            //                Student prof = new Student()
            //                {
            //                    Name = name
            //                };

            //                ApplicationUser user = new ApplicationUser()
            //                {
            //                    UserName = userName,
            //                    Email = userName + "@gmail.com",
            //                    PhoneNumber = RandomDigits(10)

            //                };

            //                this.SeedStudentApplicationUser(user, PASSWORD);

            //                prof.ApplicationUser = user;

            //                context.Students.Add(prof);

            //                context.SaveChanges();

            //            }






            //        }

            //        private void SeedStudentApplicationUser(ApplicationUser user, string password)
            //        {
            //            if (!this.roleManager.RoleExists(GlobalConstants.StudentRoleName))
            //            {
            //                this.roleManager.Create(new IdentityRole(GlobalConstants.StudentRoleName));
            //            }



            //            var result = this.userManager.Create(user, password);

            //            if (result.Succeeded)
            //            {
            //                this.userManager.AddToRole(user.Id, GlobalConstants.StudentRoleName);
            //            }
            //        }

            //        public string RandomDigits(int length)
            //        {
            //            var random = new Random();
            //            string s = string.Empty;
            //            for (int i = 0; i < length; i++)
            //                s = String.Concat(s, random.Next(10).ToString());
            //            return s;
            //        }

            //        private void SeedTeachers(ApplicationDbContext context)
            //        {

            //            if (context.Teachers.Any())
            //            {
            //                return;
            //            }

            //            for (int i = 0; i < teacherNames.Count; i++)
            //            {
            //                string name = teacherNames.ElementAt(i);
            //                string userName = teacherUserNames.ElementAt(i);

            //                Teacher prof = new Teacher()
            //                {
            //                    Name = name
            //                };

            //                ApplicationUser user = new ApplicationUser()
            //                {
            //                    UserName = userName,
            //                    Email = userName + "@gmail.com",
            //                    PhoneNumber = RandomDigits(10)

            //                };

            //                this.SeedTeacherApplicationUser(user, PASSWORD);

            //                prof.ApplicationUser = user;

            //                context.Teachers.Add(prof);

            //                context.SaveChanges();

            //            }
            //        }





            //        private void SeedTeacherApplicationUser(ApplicationUser user, string password)
            //        {
            //            if (!this.roleManager.RoleExists(GlobalConstants.TeacherRoleName))
            //            {
            //                this.roleManager.Create(new IdentityRole(GlobalConstants.TeacherRoleName));
            //            }



            //            var result = this.userManager.Create(user, password);

            //            if (result.Succeeded)
            //            {
            //                this.userManager.AddToRole(user.Id, GlobalConstants.TeacherRoleName);


            //            }
            //        }


            //        private void SeedSchoolClasses(ApplicationDbContext context)
            //        {

            //            if (context.SchoolClasses.Any())
            //            {
            //                return;
            //            }

            //            IQueryable<Student> rtn = from temp in context.Students select temp;
            //            var students = new Queue<Student>(rtn.ToList());

            //            IQueryable<Teacher> rtn2 = from temp in context.Teachers select temp;
            //            var teachers = new Queue<Teacher>(rtn2.ToList());

            //            const int studentsPerClass = 3;
            //            const int teachersPerClass = 3;
            //            foreach (KeyValuePair<string, int> pair in SchoolClasses)
            //            {

            //                SchoolClass sc = new SchoolClass() { ClassLetter = pair.Key, ClassNumber = pair.Value };



            //                foreach (Subject s in context.Subjects)
            //                {
            //                    sc.Subjects.Add(s);
            //                }

            //                context.SchoolClasses.AddOrUpdate(sc);

            //                context.SaveChanges();


            //                SchoolClass scPersistant = context.SchoolClasses.Where(x => (x.ClassNumber == pair.Value && x.ClassLetter == pair.Key)).FirstOrDefault();

            //                for (int i = 0; i < studentsPerClass; i++)
            //                {

            //                    Student s = students.Dequeue();
            //                    scPersistant.Students.Add(s);
            //                    s.SchoolClass = scPersistant;
            //                    context.Students.AddOrUpdate(s);

            //                }

            //                for (int i = 0; i < teachersPerClass; i++)
            //                {
            //                    Teacher t = teachers.Dequeue();
            //                    scPersistant.Teachers.Add(t);
            //                    t.SchoolClasses.Add(scPersistant);
            //                    context.Teachers.AddOrUpdate(t);
            //                }


            //                context.SchoolClasses.AddOrUpdate(sc);

            //            }

            //            context.SaveChanges();


            //        }


            //        private void SeedTexts(ApplicationDbContext context)
            //        {
            //            if (context.Texts.Any())
            //            {
            //                return;
            //            }


            //            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();
            //            for (int i = 0; i < textStrings.Count; i++)
            //            {
            //                pairs.Add(new KeyValuePair<string, string>(textNames.ElementAt(i), textStrings.ElementAt(i)));
            //            }

            //            foreach (KeyValuePair<string, string> pair in pairs)
            //            {
            //                Random rnd = new Random();
            //                int r = rnd.Next(SubjectNames.Count);
            //                string sName = SubjectNames.ElementAt(r);



            //                Subject subject = context.Subjects.Where(x => x.Name == sName).FirstOrDefault();

            //                Text t = new Text();
            //                t.Name = pair.Key;
            //                t.Content = pair.Value;
            //                t.Subject = subject;



            //                context.Texts.Add(t);

            //                context.SaveChanges();

            //            }

            //        }

            //        //assumes only 3 texts
            //        private void SeedHomeworks(ApplicationDbContext context)
            //        {


            //            if (context.Homeworks.Any())
            //            {
            //                return;
            //            }


            //            IQueryable<Text> rtn = from temp in context.Texts select temp;
            //            var texts = new Queue<Text>(rtn.ToList());

            //            for (int i = 0; i < 3; i++)
            //            {
            //                Text t = texts.Dequeue();

            //                Subject subject = context.Subjects.Where(x => x.Id == t.Subject_Id).FirstOrDefault();//ok

            //                if (subject == null)
            //                {
            //                    subject = context.Subjects.Local.Where(x => x.Id == t.Subject_Id).FirstOrDefault();
            //                    if (subject == null)
            //                        if (System.Diagnostics.Debugger.IsAttached == false)
            //                        {

            //                            System.Diagnostics.Debugger.Launch();

            //                        }
            //                }

            //                string Title = HWtitles.ElementAt(i);
            //                string Description = HWdescs.ElementAt(i);
            //                DateTime dead = DateTime.Now.AddYears(1);//������
            //                Teacher creator = null;

            //                if (i == 0)
            //                    creator = context.Teachers.Where(x => x.Name == "���� ���������").FirstOrDefault();
            //                else if (i == 1)
            //                    creator = context.Teachers.Where(x => x.Name == "���� ���������").FirstOrDefault();
            //                else if (i == 2)
            //                    creator = context.Teachers.Where(x => x.Name == "����� �����").FirstOrDefault();



            //                List<Question> hwQuestions = new List<Question>();//not the prettiest but oh well



            //                if (i == 0)
            //                {
            //                    hwQuestions.Add(questions.ElementAt(0));
            //                    hwQuestions.Add(questions.ElementAt(1));
            //                    hwQuestions.Add(questions.ElementAt(2));
            //                }
            //                else if (i == 1)
            //                {
            //                    hwQuestions.Add(questions.ElementAt(3));
            //                    hwQuestions.Add(questions.ElementAt(4));
            //                    hwQuestions.Add(questions.ElementAt(5));
            //                }
            //                else if (i == 2)
            //                {
            //                    hwQuestions.Add(questions.ElementAt(6));
            //                    hwQuestions.Add(questions.ElementAt(7));
            //                    hwQuestions.Add(questions.ElementAt(8));
            //                    hwQuestions.Add(questions.ElementAt(9));

            //                }



            //                Homework hw = new Homework();
            //                hw.Title = Title;
            //                hw.Description = Description;
            //                hw.Deadline = dead;
            //                hw.Created_By = creator;
            //                hw.Text = t;

            //                //if (System.Diagnostics.Debugger.IsAttached == false)
            //                //{

            //                //    System.Diagnostics.Debugger.Launch();

            //                //}hgfdhg

            //                foreach (Question q in hwQuestions)
            //                {
            //                    int till = new Random().Next(suggested_openings.Count);

            //                    for(int l=0;l<till;l++)
            //                        q.Suggested_Openings.Add(new SuggestedOpening(suggested_openings.ElementAt(new Random().Next(suggested_openings.Count))));

            //                    hw.Questions.Add(q);

            //                }

            //                context.Homeworks.Add(hw);

            //                context.SaveChanges();

            //                //ok
            //                SchoolClass getHomeworked = context.Teachers.Where(x => x.Id == creator.Id).Include(teac => teac.SchoolClasses).FirstOrDefault().SchoolClasses.FirstOrDefault();

            //                getHomeworked.Homeworks.Add(hw);
            //                var stus = context.SchoolClasses.Where(x => x.Id == getHomeworked.Id).Include(sch => sch.Students).FirstOrDefault().Students;
            //                foreach (Student s in stus)
            //                {
            //                    s.Homeworks.Add(hw);
            //                    context.Students.AddOrUpdate(s);
            //                }

            //                context.SchoolClasses.AddOrUpdate(getHomeworked);
            //                context.SaveChanges();

            //            }
            //        }

            //        private void SeedAnswers(ApplicationDbContext context)
            //        {
            //            if (context.Answers.Any())
            //            {
            //                return;
            //            }

            //            //if (System.Diagnostics.Debugger.IsAttached == false)
            //            //{

            //            //    System.Diagnostics.Debugger.Launch();

            //            //}

            //            IQueryable<Student> rtn = from temp in context.Students.Include(s => s.Homeworks) select temp;
            //            var students = new Queue<Student>(rtn.ToList());

            //            foreach(Student s in students)
            //            {
            //                Homework ans_to = s.Homeworks.FirstOrDefault();//chickitty check

            //                if (ans_to == null)//either a weird error, or the student has no homework
            //                    continue;

            //                IEnumerable<Question> questionsOfHw = context.Homeworks.
            //                    Where(x => x.Id == ans_to.Id).Include(home => home.Questions).FirstOrDefault().Questions;//chleck

            //                Answer ans = new Answer();
            //                ans.Answer_To = ans_to;
            //                ans.Submitted_By = s;
            //                int k = 1;
            //                foreach (Question q in questionsOfHw)
            //                {
            //                    ans.QuestionNumber = k;
            //                    ans.QuestionAnswer = "����� �������� ����� �������� ��� ��� ���";
            //                    k++;
            //                }

            //                context.Answers.Add(ans);
            //            }

            //            context.SaveChanges();

            //        }

            //        public string RandomAnswerString()
            //        {
            //            return answerContents.ElementAt(new Random().Next(answerContents.Count));

            //        }

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
