namespace Frontend.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common;
    using Entities.Models;
    using Services.Interfaces;
    using Frontend.Areas.Administration.Models.Account;
    using Frontend.Areas.Administration.Models.SchoolClasses;
    using Frontend.Infra;
    using System.Data.Entity;
    using PagedList;
    using Models.Students;
    using Models.Teachers;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class SchoolClassesController : Controller
    {
        private readonly ISchoolClassService schoolClassService;
        private readonly IStudentService studentService;
        private readonly ITeacherService teacherService;

        public SchoolClassesController(
            ISchoolClassService schoolClassService,
            IStudentService studentService,
            ITeacherService teacherService
            )
        {
            this.schoolClassService = schoolClassService;
            this.studentService = studentService;
            this.teacherService = teacherService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SchoolClassCreateSubmitModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            SchoolClass schoolClass = Mapper.Map<SchoolClassCreateSubmitModel, SchoolClass>(model);

            this.schoolClassService.Add(schoolClass);

            return RedirectToAction("Index", "SchoolClasses");
        }

          public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "name" ? "name_desc" : "name";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            IQueryable<SchoolClass> classes = this.schoolClassService.All().Include(x => x.Students).Include(y => y.Teachers);

            if (!string.IsNullOrEmpty(searchString))
            {
                classes = classes
                    .Where(s => searchString.Contains(s.ClassLetter) || searchString.Contains(s.ClassNumber.ToString()));
            }

            switch (sortOrder)
            {
           
                case "name":
                    classes = classes.OrderBy(s => s.ClassLetter);
                    break;
                case "name_desc":
                    classes = classes.OrderByDescending(s => s.ClassLetter);
                    break;
                default:
                    classes = classes.OrderBy(s => s.ClassLetter);
                    break;
            }

            IQueryable<SchoolClassesListViewModel> sortedSCs = classes.Project().To<SchoolClassesListViewModel>();

            int pageSize = 10;
            int pageIndex = page ?? 1;

            RedirectUrl redirectUrl = new RedirectUrl(this.ControllerContext, null);

            Session["redirectUrl"] = redirectUrl;

            return View(sortedSCs.ToPagedList(pageIndex, pageSize));
        }

        public ActionResult Details(int gradeYear, string letter)
        {
            SchoolClass schoolClass = this.schoolClassService.GetByDetails(gradeYear, letter);

            if (schoolClass == null)
            {
                return RedirectToAction("Index", "SchoolClasses");
            }

            SchoolClassDetailsEditModel schoolClassModel =
                Mapper.Map<SchoolClass, SchoolClassDetailsEditModel>(schoolClass);

            RouteValueDictionary routeParameters = new RouteValueDictionary
            {
               { "gradeYear", gradeYear },
               { "letter", letter }
            };

            RedirectUrl redirectUrl = new RedirectUrl(this.ControllerContext, routeParameters);

            this.Session["redirectUrl"] = redirectUrl;

            return View(schoolClassModel);
        }

        public ActionResult Students(Guid id, string sortOrder, string currentFilter, string searchString, int? page)
        {
            SchoolClass schoolClass = this.schoolClassService.GetById(id);
            SchoolClassStudentsView momo = new SchoolClassStudentsView();
            momo.schoolClass = schoolClass;

            if (schoolClass == null)
            {
                ModelState.AddModelError(string.Empty, "Such a class does not exist");

                var redir = Session["redirectUrl"] as RedirectUrl;

                redir = redir ?? new RedirectUrl();

                return RedirectToAction(
                    redir.RedirectActionName,
                    redir.RedirectControllerName,
                    redir.RedirectParameters);
            }


            ViewBag.CurrentSort = sortOrder;
            ViewBag.UserNameSortParam = string.IsNullOrEmpty(sortOrder) ? "username_desc" : string.Empty;
            ViewBag.NameSortParam = sortOrder == "name" ? "name_desc" : "name";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            IQueryable<Student> students = this.schoolClassService.GetStudents(schoolClass).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                students = students
                    .Where(s => s.ApplicationUser.UserName.Contains(searchString) || s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "username_desc":
                    students = students.OrderByDescending(s => s.ApplicationUser.UserName);
                    break;
                case "name":
                    students = students.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    students = students.OrderByDescending(s => s.Name);
                    break;
                default:
                    students = students.OrderBy(s => s.ApplicationUser.UserName);
                    break;
            }

            IQueryable<StudentListViewModel> sortedStudents = students.Project().To<StudentListViewModel>();

            int pageSize = 10;
            int pageIndex = page ?? 1;

            RedirectUrl redirectUrl = new RedirectUrl(this.ControllerContext, null);

            Session["redirectUrl"] = redirectUrl;
            momo.studentsListViews = sortedStudents.ToPagedList(pageIndex, pageSize);
            return View(momo);


        }

        public ActionResult Teachers(Guid id, string sortOrder, string currentFilter, string searchString, int? page)
        {
            SchoolClass schoolClass = this.schoolClassService.GetById(id);
            SchoolClassTeachersView momo = new SchoolClassTeachersView();
            momo.schoolClass = schoolClass;

            if (schoolClass == null)
            {
                ModelState.AddModelError(string.Empty, "Such a class does not exist");

                var redir = Session["redirectUrl"] as RedirectUrl;

                redir = redir ?? new RedirectUrl();

                return RedirectToAction(
                    redir.RedirectActionName,
                    redir.RedirectControllerName,
                    redir.RedirectParameters);
            }


            ViewBag.CurrentSort = sortOrder;
            ViewBag.UserNameSortParam = string.IsNullOrEmpty(sortOrder) ? "username_desc" : string.Empty;
            ViewBag.NameSortParam = sortOrder == "name" ? "name_desc" : "name";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            IQueryable<Teacher> teachers = this.schoolClassService.GetTeachers(schoolClass).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                teachers = teachers
                    .Where(s => s.ApplicationUser.UserName.Contains(searchString) || s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "username_desc":
                    teachers = teachers.OrderByDescending(s => s.ApplicationUser.UserName);
                    break;
                case "name":
                    teachers = teachers.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    teachers = teachers.OrderByDescending(s => s.Name);
                    break;
                default:
                    teachers = teachers.OrderBy(s => s.ApplicationUser.UserName);
                    break;
            }

            IQueryable<TeacherListViewModel> sortedTeachers = teachers.Project().To<TeacherListViewModel>();

            int pageSize = 10;
            int pageIndex = page ?? 1;

            RedirectUrl redirectUrl = new RedirectUrl(this.ControllerContext, null);

            Session["redirectUrl"] = redirectUrl;
            momo.teachersListViews = sortedTeachers.ToPagedList(pageIndex, pageSize);
            return View(momo);


        }

        [HttpGet]
        public ActionResult AddStudentToClass(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                ModelState.AddModelError(string.Empty, "No class has been selected");
                return View();
            }

            SchoolClass sc = this.schoolClassService.GetById(id);

            if (sc == null)
            {
                ModelState.AddModelError(string.Empty, "Such a class does not exist");
                return View();
            }


            AddStudentToClassViewModel newModel = new AddStudentToClassViewModel();

            AddStudentToClassViewModel schoolClassModel =
               Mapper.Map<SchoolClass, AddStudentToClassViewModel>(sc);

            return View(schoolClassModel);
        }

        [HttpGet]
        public ActionResult AddTeacherToClass(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                ModelState.AddModelError(string.Empty, "No class has been selected");
                return View();
            }

            SchoolClass sc = this.schoolClassService.GetById(id);

            if (sc == null)
            {
                ModelState.AddModelError(string.Empty, "Such a class does not exist");
                return View();
            }


            AddTeacherToClassViewModel newModel = new AddTeacherToClassViewModel();

            AddTeacherToClassViewModel schoolClassModel =
               Mapper.Map<SchoolClass, AddTeacherToClassViewModel>(sc);

            return View(schoolClassModel);
        }




        [HttpPost]
        public ActionResult AddStudentToClass(AddStudentToClassViewModel model)
        {


            string url = Request.Url.ToString();
            string[] slashes = url.Split('/');
            string id = "";
            foreach(string str in slashes)
            {
                if (str.Contains('-'))
                    id = str;
            }

            string username = model.userName;
            Student s = this.studentService.GetByUserName(username);

            if (s == null)
            {
                ModelState.AddModelError(string.Empty, "user name does not exist");
                return RedirectToAction("Students", "SchoolClasses", new { id = id });
            }
                MichtavaResult res = this.schoolClassService.AddStudentToClass(s, this.schoolClassService.GetById(new Guid(id)));

            if (res is MichtavaFailure)
                ModelState.AddModelError(string.Empty, res.Message);
            
                return RedirectToAction("Students","SchoolClasses",new {id = id}) ;

        }

        [HttpPost]
        public ActionResult AddTeacherToClass(AddTeacherToClassViewModel model)
        {


            string url = Request.Url.ToString();
            string[] slashes = url.Split('/');
            string id = "";
            foreach (string str in slashes)
            {
                if (str.Contains('-'))
                    id = str;
            }

            string username = model.userName;
            Teacher t = this.teacherService.GetByUserName(username);

            if (t == null)
            {
                ModelState.AddModelError(string.Empty, "user name does not exist");
                return RedirectToAction("Teachers", "SchoolClasses", new { id = id });
            }
            MichtavaResult res = this.schoolClassService.AddTeacherToClass(t, this.schoolClassService.GetById(new Guid(id)));

            if (res is MichtavaFailure)
                ModelState.AddModelError(string.Empty, res.Message);

            return RedirectToAction("Teachers", "SchoolClasses", new { id = id });

        }

        public ActionResult StudentDetails(Guid id, string username)
        {

            Student s = this.studentService.GetByUserName(username);
            SchoolClass schoolClass = this.schoolClassService.GetById(id);


            if (schoolClass == null)
            {
                return RedirectToAction("Index", "SchoolClasses");
            }

            StudentDetailsEditModel studentModel =
                Mapper.Map<Student, StudentDetailsEditModel>(s);
            studentModel.AccountDetailsEditModel = Mapper.Map<ApplicationUser, AccountDetailsEditModel>(
                s.ApplicationUser);

            return View(studentModel);

        }

        public ActionResult TeacherDetails(Guid id, string username)
        {

            Teacher t = this.teacherService.GetByUserName(username);
            SchoolClass schoolClass = this.schoolClassService.GetById(id);


            if (schoolClass == null)
            {
                return RedirectToAction("Index", "SchoolClasses");
            }

            TeacherDetailsEditModel teacherModel =
                Mapper.Map<Teacher, TeacherDetailsEditModel>(t);
            teacherModel.AccountDetailsEditModel = Mapper.Map<ApplicationUser, AccountDetailsEditModel>(
                t.ApplicationUser);

            return View(teacherModel);

        }


        public ActionResult RemoveStudentFromClass(Guid id,string username)
        {
            Student s = this.studentService.GetByUserName(username);
            SchoolClass sc = schoolClassService.GetById(id);
            MichtavaResult res = this.schoolClassService.RemoveStudentFromClass(s, sc);

            if (res is MichtavaFailure)
                ModelState.AddModelError(string.Empty, res.Message);

            return RedirectToAction("Students", "SchoolClasses", new { id = sc.Id });
        }

        public ActionResult RemoveTeacherFromClass(Guid id, string username)
        {
            Teacher t = this.teacherService.GetByUserName(username);
            SchoolClass sc = schoolClassService.GetById(id);
            MichtavaResult res = this.schoolClassService.RemoveTeacherFromClass(t, sc);

            if (res is MichtavaFailure)
                ModelState.AddModelError(string.Empty, res.Message);

            return RedirectToAction("Teachers", "SchoolClasses", new { id = sc.Id });
        }

        //public ActionResult Teachers(Guid id, string sortOrder, string currentFilter, string searchString, int? page)
        //{

        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(SchoolClassEditViewModel model)
        //{
        //    SchoolClass schoolClass = this.schoolClassService.GetById(model.Id);

        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    Mapper.Map<SchoolClassEditViewModel, SchoolClass>(model, schoolClass);

        //    Grade grade = this.gradeService.All().FirstOrDefault(g =>
        //        g.GradeYear == model.GradeYear &&
        //        g.AcademicYearId == schoolClass.Grade.AcademicYearId);

        //    grade = grade ?? new Grade();

        //    schoolClass.GradeId = grade.Id;

        //    this.schoolClassService.Update(schoolClass);

        //    var redirectUrl = Session["redirectUrl"] as RedirectUrl;

        //    redirectUrl = redirectUrl ?? new RedirectUrl();

        //    return RedirectToAction(
        //        redirectUrl.RedirectActionName,
        //        redirectUrl.RedirectControllerName,
        //        redirectUrl.RedirectParameters);
        //}

        public ActionResult Delete(Guid id)
        {
            SchoolClass schoolClass = this.schoolClassService.GetById(id);


            return View(schoolClass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            SchoolClass schoolClass = this.schoolClassService.GetById(id);

            MichtavaResult res = this.schoolClassService.Delete(schoolClass);

            if(res is MichtavaFailure)
                ModelState.AddModelError(string.Empty, res.Message);

            var redirectUrl = Session["redirectUrl"] as RedirectUrl;

            redirectUrl = redirectUrl ?? new RedirectUrl();

            return RedirectToAction(
                redirectUrl.RedirectActionName,
                redirectUrl.RedirectControllerName,
                redirectUrl.RedirectParameters);
        }
    }
}