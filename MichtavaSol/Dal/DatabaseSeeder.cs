using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Entities.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace Dal
{
    public class DatabaseSeeder
    {
        //superadmin: U:superadmin P:111
        //admin: U:admin P:111



        public UserManager<ApplicationUser> userManager;

        public RoleManager<IdentityRole> roleManager;

        #region consts
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


        private static List<SuggestedOpening> suggested_openings = new List<SuggestedOpening>() {
            new SuggestedOpening("ההשלכות למעשיה הן להלן:"),
             new SuggestedOpening("ריקודי החורף מגיעים בין התאריכים:"),
             new SuggestedOpening("this is a suggested opening in english"),
             new SuggestedOpening(" ריקושט הם יותר יקרים משאר חנויות המטיילים מכיוון ש"),
             new SuggestedOpening("התשובה לשאלה היא התשובה הבאה:"),
              new SuggestedOpening("ההשלכות של המעשים של אליס הם להלן;"),
              new SuggestedOpening("ההשלכות של מעשי אליס הן: \n 1)טרה לה לה")
        };




        private readonly List<Question> questions = new List<Question>()
        {
            new Question("הסבר מהם ההשלכות של הדברים שאמרה אליס מקארת'י"),
            new Question("מנה את כל הדרכים בהן אליס עברה על החוק הרומני:"),
            new Question("רציתי להגיד לך שאני מאוהב בך בטירוף, ולא לא עשית ____? (השלם את החסר)"),
            new Question("באיזה שנה \"גילה\" קולומבוס את העולם החדש, לרוע מזלם של המקומיים?"),
            new Question("איזו שרת כנסת בכירה נכחה בהפגנה נגד הקהילה הלהטבית?"),
            new Question("מה היא בירת קרואטיה (2017)?"),
            new Question("this is a question in english"),
            new Question("מי האמן הכי רווחי בכל הזמנים, בכל העולם?"),
            new Question("מי האמן הכי רווחי בכל הזמנים, בכל העולם?"),
            new Question("מה היה הצבע של העץ מפלסטיק שקנו למרגולית צנעני?"),

        };

        private readonly List<string> answerContents = new List<string>()
        {
            "ההשלכות הן אכן מרחיקות לכת, אך לא נפרט על כך פה",
           "אליס עברה את העבירות הבאות: חוסר רפלקסיביות, שאננות בעת מילוי תפקיד, והזנחת רכשים שלא לצורך",
           "כישוף מינימלי עולמות מקבילים צ'יקו ודיקו חברים הכי קרובים",
           "לדעתי זה קרה בשנת 1492 וזה מצער נורא מצד שני בגלל זה יש לנו מקדונלדס וכדומה",
           "בואו נגיד שזאת לא זהבה גלאון כי היא מלאכית שרת",
           "התשובה לשאלה הנ\"ל היא זאגרב, כדאי לבקר מומלץ בקור",
           "this is an answer  in english",
           "התשובה היא כנראה אלביס פרסלי, אבל אין לדעת מה טומן בחובו הגורל.",
            "התשובה היא כנראה מרגולית צנעני, שעבור הסינגל עץ פיסטוק ירוק גרפה הזמרת 2 מיליארד שקלים נקי ממס",
          "וקנית לי עץ ירוק מפלסטיק, היית בטוח שהכל מתברך "

        };

        private readonly List<string> words = new List<string>() {
        "צורותיהן",
        "ספרה",
        "כעכים",
        "מיקסר",
        "טעם לוואי",
        "התפחה",
        };

        private readonly List<List<string>> definitionLists = new List<List<string>>() {
        new List<string>() { "הצורה שלהן" },
        new List<string>() { "מספר כלשהו בין 0 ל9, כולל" },
        new List<string>() { "בייגלה","לחם בצורת טבעת העשויה מקמח חיטה" },
        new List<string>() { "מכשיר לערבוב חומרים, בד\"כ נוזלים " },
        new List<string>() { "טעם שלא תואם את הטעם המצופה של המנה" ,"טעם לא טוב ולא מתוכנן"},
        new List<string>() { "הבאת הבצק\\בלילה לכדי צמיחה","החדרת אוויר לבצק או בלילה על מנת שהמוצר המוגמר יהיה רך וקל" },
        };





        private const string PASSWORD = "111";

        private readonly DateTime startDate = new DateTime(2016, 3, 22);

        private readonly DateTime endDate = new DateTime(2017, 3, 22);

        #endregion
        public void CreateDependenciesAndSeed(ApplicationDbContext context)
        {
            this.userManager = this.CreateUserManager(context);
            this.roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));


            Seed(context, userManager, roleManager);

        }


        public void Seed(ApplicationDbContext context,UserManager<ApplicationUser> um,RoleManager<IdentityRole> rm)
        {
            //this.userManager = this.CreateUserManager(context);
            //this.roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            this.userManager = um;
            this.roleManager = rm;

            context.Configuration.AutoDetectChangesEnabled = false;

            this.SeedRoles(context);
            this.SeedSubjects(context);
            this.SeedAdministrators(context);


            this.SeedStudents(context);
            this.SeedTeachers(context);
            this.SeedSchoolClasses(context);
            this.SeedTexts(context);
            this.SeedHomeworks(context);
            this.SeedAnswers(context);
            this.SeedDefinitions(context);
            //fixseed

            context.Configuration.AutoDetectChangesEnabled = true;

        }

        
        private void SeedSubjects(ApplicationDbContext context)
        {
            if (context.Subjects.Any())
                return;

            foreach (var subjectName in SubjectNames)
            {
                Subject subject = new Subject(subjectName);
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

           

            var adminUser = new ApplicationUser()
            {
                UserName = "superadmin",
                Email = "superadmin@superadmin.com"
            };



            this.SeedAdminApplicationUser(adminUser, PASSWORD);

            var superAdminProfile = new Administrator(adminUser,"SuperAdmin", "SuperAdmin");

            context.Administrators.Add(superAdminProfile);


           

            adminUser = new ApplicationUser()
            {
                UserName = "admin",
                Email = "admin@admin.com"
            };

            this.SeedAdminApplicationUser(adminUser, PASSWORD);

            var adminProfile = new Administrator(adminUser,"Admin", "Admin");

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
                t.Games.Add(new ExternalGame("www.examplegame.co.il/game", "Example game"));


                context.Texts.Add(t);

                context.SaveChanges();

            }

        }

        //assumes only 3 texts
        private void SeedHomeworks(ApplicationDbContext context)
        {


            if (context.Homeworks.Any())
            {
                return;
            }


            IQueryable<Text> rtn = from temp in context.Texts select temp;
            var texts = new Queue<Text>(rtn.ToList());

            for (int i = 0; i < 3; i++)
            {
                Text t = texts.Dequeue();

                Subject subject = context.Subjects.Where(x => x.Id == t.Subject_Id).FirstOrDefault();//ok

                if (subject == null)
                {
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

                //if (System.Diagnostics.Debugger.IsAttached == false)
                //{

                //    System.Diagnostics.Debugger.Launch();

                //}hgfdhg

                foreach (Question q in hwQuestions)
                {
                    int till = new Random().Next(suggested_openings.Count);

                    for (int l = 0; l < till; l++)
                        q.Suggested_Openings.Add(new SuggestedOpening(suggested_openings.ElementAt(new Random().Next(suggested_openings.Count))));

                    hw.Questions.Add(q);

                }

                context.Homeworks.Add(hw);

                context.SaveChanges();

                //ok
                SchoolClass getHomeworked = context.Teachers.Where(x => x.Id == creator.Id).Include(teac => teac.SchoolClasses).FirstOrDefault().SchoolClasses.FirstOrDefault();

                getHomeworked.Homeworks.Add(hw);
                var stus = context.SchoolClasses.Where(x => x.Id == getHomeworked.Id).Include(sch => sch.Students).FirstOrDefault().Students;
                foreach (Student s in stus)
                {
                    s.Homeworks.Add(hw);
                    context.Students.AddOrUpdate(s);
                }

                context.SchoolClasses.AddOrUpdate(getHomeworked);
                context.SaveChanges();

            }
        }

        private void SeedAnswers(ApplicationDbContext context)
        {
            if (context.Answers.Any())
            {
                return;
            }

            //if (System.Diagnostics.Debugger.IsAttached == false)
            //{

            //    System.Diagnostics.Debugger.Launch();

            //}

            IQueryable<Student> rtn = from temp in context.Students.Include(s => s.Homeworks) select temp;
            var students = new Queue<Student>(rtn.ToList());

            foreach (Student s in students)
            {
                Homework ans_to = s.Homeworks.FirstOrDefault();//chickitty check

                if (ans_to == null)//either a weird error, or the student has no homework
                    continue;

                IEnumerable<Question> questionsOfHw = context.Homeworks.
                    Where(x => x.Id == ans_to.Id).Include(home => home.Questions).FirstOrDefault().Questions;//chleck


                Answer ans = new Answer();
                ans.Answer_To = ans_to;
                ans.Submitted_By = s;

                foreach (Question q in questionsOfHw)
                {
                    QuestionAnswer qa = new QuestionAnswer(q, answerContents.ElementAt(new Random().Next(answerContents.Count)));
                    qa.In_Answer = ans;
                    ans.questionAnswers.Add(qa);
                }

                context.Answers.Add(ans);
            }

            context.SaveChanges();

        }

        private void SeedDefinitions(ApplicationDbContext context)
        {
            //if (context.Definitions.Any())
            //{
            //    return;
            //}

            //if (System.Diagnostics.Debugger.IsAttached == false)
            //{

            //    System.Diagnostics.Debugger.Launch();

            //}


            for (int j=0;j<words.Count;j++)
            {

                WordDefinition def = new WordDefinition();
                def.Word = words.ElementAt(j);
                def.addDefinitions(definitionLists.ElementAt(j));

                context.Definitions.AddOrUpdate(def);
            }

            context.SaveChanges();
        }



        public string RandomAnswerString()
        {
            return answerContents.ElementAt(new Random().Next(answerContents.Count));

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

    }
}

    

