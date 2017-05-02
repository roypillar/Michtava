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

    internal sealed class Configuration : DbMigrationsConfiguration<Dal.ApplicationDbContext>, IDisposable
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

        private static readonly string text1 =

        "כדי להצליח בהתפחה, מונעים מגע ישיר בין השמרים לשמן זית או מלח, שעלולים לפגוע בפעולתם.מכאן יצא המנהג להתסיס את השמרים בסוכר ומים לפני שמתחילים את הבצק.אבל זה לא הכרחי אלא אם אתם חושדים בטריותם, ואז זה טסט מצוין (אין בועות אחרי 10 דקות? אפשר להתחיל שוב)." + "\n\n" +

"מתחילים מערבבים שמרים עם קמח ומים(עדיף פושרים), ואם יש במתכון — אז גם סוכר או דבש.רק כאשר השמרים כבר מעורבבים בנוח בקערה מוסיפים את שאר הרכיבים כולל מלח ושמן." + "\n" +

"לשים גם אם אתם מכינים את הבצק במיקסר, אל תוותרו על זמני הלישה." + "\n" +

"מכסים רצוי לכסות בניילון נצמד ובמגבת מעל (וכך המגבת לא תידבק), כדי שיהיה לשמרים חמים וגם קצת חשוך." + "\n" +

"מקום חמים לא מתפיחים מול המזגן! מתפיחים במקום חמים עד שעה וחצי להכפלת הנפח.אל תגזימו, כי לבצק שמרים שתפח יותר מדי יש טעם לוואי." + "\n" +

"מקום קריר כשמכינים מראש אפשר להתפיח במקרר במשך 8 שעות." + "\n" +

"אל תגזימו, כי לבצק שמרים שתפח יותר מדי יש טעם לוואי.";

        private readonly List<string> textStrings = new List<string>(){
            text1,
            "אבא ואמא אומרים לבחור טוב בין קז'ואל פריידי ללבוש פורמאלי",
            "טקסט שלישי חמאת בוטנים? לא תודה ענה אבינועם. לאחר מכן,\n\n "+"בחר שללכת לנוח בחדר התינוקות השכונתי."
        };

        private readonly List<string> textNames = new List<string>()
        {
            "מתכון לפוקאצ'ה מתקדמת",
            "מה ללבוש היום?",
            "בחירתו של אבינועם"
        };

        private readonly List<string> HWtitles = new List<string>(){
            text1,
            "תקופת הרנסנס: תחיה מחדש",
            "השכלה גבוה: המסלול הבטוח להצלחה?",
            "הפיצה וחשיבותה לתושבי שמעוני 3א"
        };

        private readonly List<string> HWdescs = new List<string>(){
            text1,
            "תקופת הרנסנס שינתה את פניה של אירופה של המאה 17. מה אתם יודעים על התקופה הססגונית הזאת?",
            "ביל גייטס, סטיב ג'ובות,ועוד אלפי יזמים מצליחים עזבו את הואר בטרם סיימו אותו. האם האוניברסיטה היא אכן הכלי האולטימטיבי לפאוור ריינג'רס?",
            "ענו על מספר שאלות הקשורות בפיצה ורחוב חשוב בשכונה ב' בבאר שבע מיקוד 44444."
        };


        private static readonly HashSet<string> suggested_openings = new HashSet<string>() {
            "ההשלכות למעשיה הן להלן:",
            "ריקודי החורף מגיעים בין התאריכים:",
            "this is a suggested opening in english",
            " ריקושט הם יותר יקרים משאר חנויות המטיילים מכיוון ש",
            "התשובה לשאלה היא התשובה הבאה:",
             "ההשלכות של המעשים של אליס הם להלן;",
             "ההשלכות של מעשי אליס הן: \n 1)טרה לה לה"
        };


        private readonly List<Question> questions = new List<Question>()
        {
            new Question("הסבר מהם ההשלכות של הדברים שאמרה אליס מקארת'י",suggested_openings),
            new Question("מנה את כל הדרכים בהן אליס עברה על החוק הרומני:",suggested_openings),
            new Question("רציתי להגיד לך שאני מאוהב בך בטירוף, ולא לא עשית ____? (השלם את החסר)",suggested_openings),
            new Question("באיזה שנה \"גילה\" קולומבוס את העולם החדש, לרוע מזלם של המקומיים?",suggested_openings),
            new Question("איזו שרת כנסת בכירה נכחה בהפגנה נגד הקהילה הלהטבית?",suggested_openings),
            new Question("מה היא בירת קרואטיה (2017)?",suggested_openings),
            new Question("this is a question in english",suggested_openings),
            new Question("מי האמן הכי רווחי בכל הזמנים, בכל העולם?",suggested_openings),
            new Question("מי האמן הכי רווחי בכל הזמנים, בכל העולם?",suggested_openings),
            new Question("מה היה הצבע של העץ מפלסטיק שקנו למרגולית צנעני?",suggested_openings),

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
            this.SeedTexts(context);
            this.SeedHomeworks(context);
            //this.SeedAnswers(context);


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



            for (int i = 0; i < studentNames.Count; i++)
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

            if (context.SchoolClasses.Any())
            {
                return;
            }

            IQueryable<Student> rtn = from temp in context.Students select temp;
            var students = new Queue<Student>(rtn.ToList());

            IQueryable<Teacher> rtn2 = from temp in context.Teachers select temp;
            var teachers = new Queue<Teacher>(rtn2.ToList());

            const int studentsPerClass = 3;
            const int teachersPerClass = 3;
            foreach (KeyValuePair<string, int> pair in SchoolClasses)
            {

                SchoolClass sc = new SchoolClass() { ClassLetter = pair.Key, ClassNumber = pair.Value };



                foreach (Subject s in context.Subjects)
                {
                    sc.Subjects.Add(s);
                }

                context.SchoolClasses.AddOrUpdate(sc);

                context.SaveChanges();


                SchoolClass scPersistant = context.SchoolClasses.Where(x => (x.ClassNumber == pair.Value && x.ClassLetter == pair.Key)).FirstOrDefault();

                for (int i = 0; i < studentsPerClass; i++)
                {

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




            }

            context.SaveChanges();

          
        }


        private void SeedTexts(ApplicationDbContext context)
        {
            if (context.Texts.Any())
            {
                return;
            }


            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < textStrings.Count; i++)
            {
                pairs.Add(new KeyValuePair<string, string>(textNames.ElementAt(i), textStrings.ElementAt(i)));
            }

            foreach (KeyValuePair<string, string> pair in pairs)
            {
                Random rnd = new Random();
                int r = rnd.Next(SubjectNames.Count);
                string sName = SubjectNames.ElementAt(r);

               

                Subject subject = context.Subjects.Where(x => x.Name == sName).FirstOrDefault();

                Text t = new Text();
                t.Name = pair.Key;
                t.Content = pair.Value;
                t.Subject = subject;


          
                context.Texts.Add(t);

                context.SaveChanges();

            }

        }

        //assumes only 3 texts
        private void SeedHomeworks(ApplicationDbContext context)
        {

        

            IQueryable<Text> rtn = from temp in context.Texts select temp;
            var texts = new Queue<Text>(rtn.ToList());

            for (int i = 0; i < 3; i++)
            {
                Text t = texts.Dequeue();

                Subject subject = context.Subjects.Where(x => x.Id == t.Subject_Id).FirstOrDefault();

                if (subject == null) {
                 subject = context.Subjects.Local.Where(x => x.Id == t.Subject_Id).FirstOrDefault();
                    if (subject == null)
                        if (System.Diagnostics.Debugger.IsAttached == false)
                        {

                            System.Diagnostics.Debugger.Launch();

                        }
                }

                string Title = HWtitles.ElementAt(i);
                string Description = HWdescs.ElementAt(i);
                DateTime dead = DateTime.Now.AddYears(1);//הלוואי
                Teacher creator = null;

                if (i == 0)
                    creator = context.Teachers.Where(x => x.Name == "פיטר מקלאופלאן").FirstOrDefault();
                else if (i == 1)
                    creator = context.Teachers.Where(x => x.Name == "פיטר מקלאופלאן").FirstOrDefault();
                else if (i == 2)
                    creator = context.Teachers.Where(x => x.Name == "מיכאל קאיסר").FirstOrDefault();



                List<Question> hwQuestions = new List<Question>();//not the prettiest but oh well

                if (i == 0)
                {
                    hwQuestions.Add(questions.ElementAt(0));
                    hwQuestions.Add(questions.ElementAt(1));
                    hwQuestions.Add(questions.ElementAt(2));
                }
                else if (i == 1)
                {
                    hwQuestions.Add(questions.ElementAt(3));
                    hwQuestions.Add(questions.ElementAt(4));
                    hwQuestions.Add(questions.ElementAt(5));
                }
                else if (i == 2)
                {
                    hwQuestions.Add(questions.ElementAt(6));
                    hwQuestions.Add(questions.ElementAt(7));
                    hwQuestions.Add(questions.ElementAt(8));
                    hwQuestions.Add(questions.ElementAt(9));
           
                }



                Homework hw = new Homework();
                hw.Title = Title;
                hw.Description = Description;
                hw.Deadline = dead;
                hw.Created_By = creator;
                hw.Text = t;

               

                foreach (Question q in hwQuestions) 
                  hw.Questions.Add(q);

                context.Homeworks.Add(hw);

                context.SaveChanges();

                //check this yo
                SchoolClass getHomeworked = context.Teachers.Include("SchoolClasses").Where(x => x.Id == creator.Id).FirstOrDefault().SchoolClasses.FirstOrDefault();

                getHomeworked.Homeworks.Add(hw);
                var stus = context.SchoolClasses.Include("Students").Where(x => x.Id == getHomeworked.Id).FirstOrDefault().Students;
                foreach(Student s in stus)
                {
                    s.Homeworks.Add(hw);
                    context.Students.AddOrUpdate(s);
                }

                context.SchoolClasses.AddOrUpdate(getHomeworked);
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
