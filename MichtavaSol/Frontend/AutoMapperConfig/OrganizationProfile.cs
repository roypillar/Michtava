namespace Frontend.AutoMapperConfig
{
    using System.Linq;
    using AutoMapper;
    using Entities.Models;

    public class OrganizationProfile : Profile
    {
        public override string ProfileName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        protected override void Configure()//this is invoked on application start
        {
           

            Mapper.CreateMap<Frontend.Areas.Students.Models.AccountViewModels.StudentRegisterViewModel, Student>();//create a map from 1 model to the other, so that later we won't have to do type: {x.name = y.name} 50 times. 


            //admin - applicationuser to accountDetails 
            Mapper.CreateMap<ApplicationUser, Frontend.Areas.Administration.Models.Account.AccountDetailsEditModel>();

            //admin - other way around
            Mapper.CreateMap<Frontend.Areas.Administration.Models.Account.AccountDetailsEditModel, ApplicationUser>();


            //registery - admins non-applicationuser attributes
            Mapper.CreateMap<Frontend.Areas.Administration.Models.Admins.AdministratorRegisterSubmitModel, Administrator>();


            //admin  - edit an admins details
            Mapper.CreateMap<Administrator, Frontend.Areas.Administration.Models.Admins.AdministratorDetailsEditModel>()
                .ForMember(dest => dest.AccountDetailsEditModel, opt => opt.MapFrom(src => src.ApplicationUser));

            Mapper.CreateMap<Frontend.Areas.Administration.Models.Admins.AdministratorDetailsEditModel, Administrator>()
                .ForMember(dest => dest.ApplicationUser, opt => opt.MapFrom(src => src.AccountDetailsEditModel));


            //admin - admins to listview of admins
            Mapper.CreateMap<Administrator, Frontend.Areas.Administration.Models.Admins.AdministratorListViewModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email));

            Mapper.CreateMap<Frontend.Areas.Administration.Models.Admins.AdministratorListViewModel, Administrator>();

            //admin - delete admin
            Mapper.CreateMap<Administrator, Frontend.Areas.Administration.Models.Admins.AdministratorDeleteSubmitModel>();

            //admin - student to studentList
            Mapper.CreateMap<Student, Frontend.Areas.Administration.Models.Students.StudentListViewModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email))
                .ForMember(dest => dest.SchoolClass, opt => opt.MapFrom(src => src.SchoolClass.ClassLetter + src.SchoolClass.ClassNumber));

            Mapper.CreateMap<Frontend.Areas.Administration.Models.Students.StudentListViewModel, Student>();

            //admin - edit students details
            Mapper.CreateMap<Student, Frontend.Areas.Administration.Models.Students.StudentDetailsEditModel>()
                                .ForMember(dest => dest.SchoolClass, opt => opt.MapFrom(src => src.SchoolClass.ClassLetter + src.SchoolClass.ClassNumber));


            //admin - details to student
            Mapper.CreateMap<Frontend.Areas.Administration.Models.Students.StudentDetailsEditModel, Student>();

            //admin - list to student
            Mapper.CreateMap<Frontend.Areas.Administration.Models.Students.StudentListViewModel, Student>();


            //admin - student to edit student
            Mapper.CreateMap<Student, Frontend.Areas.Administration.Models.Students.StudentDetailsEditModel>();

            //admin - student details to student
            Mapper.CreateMap<Frontend.Areas.Administration.Models.Students.StudentDetailsEditModel, Student>();

            //admin - teacher to teacherList
            Mapper.CreateMap<Teacher, Frontend.Areas.Administration.Models.Teachers.TeacherListViewModel>()
              .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
              .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email))
              .ForMember(dest => dest.ClassesNumber, opt => opt.MapFrom(src => src.SchoolClasses.Count()));


            //admin - teacher to teacherDetails
            Mapper.CreateMap<Teacher, Frontend.Areas.Administration.Models.Teachers.TeacherDetailsEditModel>();

            //admin - teacherDetails to teacher
            Mapper.CreateMap<Frontend.Areas.Administration.Models.Teachers.TeacherDetailsEditModel, Teacher>();


            Mapper.CreateMap<SchoolClass, Frontend.Areas.Administration.Models.SchoolClasses.SchoolClassesListViewModel>()
              .ForMember(dest => dest.ClassNumber, opt => opt.MapFrom(src => src.ClassNumber))
              .ForMember(
                  dest => dest.StudentsNumber,
                  opt => opt.MapFrom(src => src.Students.Count(s => s.IsDeleted == false)));

            //Mapper.CreateMap<SchoolClass, School.Web.Areas.Administration.Models.SchoolClassDetailsViewModel>()
            //    .ForMember(dest => dest.GradeYear, opt => opt.MapFrom(src => src.Grade.GradeYear))
            //    .ForMember(
            //        dest => dest.StudentsNumber,
            //        opt => opt.MapFrom(src => src.Students.Count(s => s.IsDeleted == false)))
            //    .ForMember(
            //        dest => dest.Students,
            //        opt => opt.MapFrom(src => src.Students.Where(s => s.IsDeleted == false)))
            //    .ForMember(dest => dest.AcademicYear, opt => opt.MapFrom(src => src.Grade.AcademicYear));

            //Mapper.CreateMap<SchoolClass, School.Web.Areas.Administration.Models.SchoolClassEditViewModel>()
            //    .ForMember(dest => dest.AcademicYearId, opt => opt.MapFrom(src => src.Grade.AcademicYearId));

            //Mapper.CreateMap<School.Web.Areas.Administration.Models.SchoolClassEditViewModel, SchoolClass>();

            Mapper.CreateMap<SchoolClass, Frontend.Areas.Administration.Models.SchoolClasses.SchoolClassCreateSubmitModel>();

            Mapper.CreateMap<Frontend.Areas.Administration.Models.SchoolClasses.SchoolClassCreateSubmitModel, SchoolClass>();

            //Mapper.CreateMap<SchoolClass, School.Web.Areas.Administration.Models.SchoolClassDeleteViewModel>()
            //    .ForMember(dest => dest.GradeYear, opt => opt.MapFrom(src => src.Grade.GradeYear))
            //    .ForMember(
            //        dest => dest.AcademicYearStartDate,
            //        opt => opt.MapFrom(src => src.Grade.AcademicYear.StartDate))
            //    .ForMember(
            //        dest => dest.AcademicYearEndDate,
            //        opt => opt.MapFrom(src => src.Grade.AcademicYear.EndDate))
            //    .ForMember(
            //        dest => dest.StudentsNumber,
            //        opt => opt.MapFrom(src => src.Students.Count()));



            //subjects - view list of subjects
            Mapper.CreateMap<Subject, Frontend.Models.SubjectsListViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SubjectsName, opt => opt.MapFrom(src => src.Name));
        }
    }
}