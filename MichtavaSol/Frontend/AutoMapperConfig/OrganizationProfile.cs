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

        protected override void Configure()
        {
            //Mapper.CreateMap<ApplicationUser, Frontend.Areas.Administration.Models.AccountDetailsEditModel>();

            //Mapper.CreateMap<Frontend.Areas.Administration.Models.AccountDetailsEditModel, ApplicationUser>();

            //Mapper.CreateMap<Frontend.Areas.Administration.Models.AdministratorRegisterSubmitModel, Administrator>();

            //Mapper.CreateMap<Administrator, Frontend.Areas.Administration.Models.AdministratorDetailsEditModel>()
            //    .ForMember(dest => dest.AccountDetailsEditModel, opt => opt.MapFrom(src => src.ApplicationUser));

            //Mapper.CreateMap<Frontend.Areas.Administration.Models.AdministratorDetailsEditModel, Administrator>()
            //    .ForMember(dest => dest.ApplicationUser, opt => opt.MapFrom(src => src.AccountDetailsEditModel));

            //Mapper.CreateMap<Administrator, Frontend.Areas.Administration.Models.AdministratorListViewModel>()
            //    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
            //    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email));

            //Mapper.CreateMap<Frontend.Areas.Administration.Models.AdministratorListViewModel, Administrator>();

            //Mapper.CreateMap<Administrator, Frontend.Areas.Administration.Models.AdministratorDeleteSubmitModel>();

            //Mapper.CreateMap<Frontend.Areas.Students.Models.StudentRegisterSubmitModel, Student>();

            //Mapper.CreateMap<Student, Frontend.Areas.Administration.Models.StudentListViewModel>()
            //    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
            //    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email));

            //Mapper.CreateMap<Frontend.Areas.Administration.Models.StudentListViewModel, Student>();

            //Mapper.CreateMap<Student, Frontend.Areas.Administration.Models.StudentDetailsEditModel>();

            //Mapper.CreateMap<Frontend.Areas.Administration.Models.StudentDetailsEditModel, Student>();

            //Mapper.CreateMap<Frontend.Areas.Teachers.Models.TeacherRegisterSubmitModel, Teacher>();

            //Mapper.CreateMap<Teacher, Frontend.Areas.Administration.Models.TeacherListViewModel>()
            //    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
            //    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email));

            //Mapper.CreateMap<Teacher, Frontend.Areas.Administration.Models.TeacherDetailsEditModel>();

            //Mapper.CreateMap<Frontend.Areas.Administration.Models.TeacherDetailsEditModel, Teacher>();


            //Mapper.CreateMap<Grade, Frontend.Areas.Administration.Models.GradeListViewModel>()
            //    .ForMember(dest => dest.SchoolClassesCount, opt => opt.MapFrom(src => src.SchoolClasses.Count))
            //    .ForMember(
            //        dest => dest.SchoolClasses,
            //        opt => opt.MapFrom(src => src.SchoolClasses.OrderBy(sc => sc.ClassLetter)));

            //Mapper.CreateMap<Frontend.Areas.Administration.Models.GradeCreateSubmitModel, Grade>();

            //Mapper.CreateMap<Grade, Frontend.Areas.Administration.Models.GradeCreateSubmitModel>()
            //    .ForMember(dest => dest.AcademicYearStartDate, opt => opt.MapFrom(src => src.AcademicYear.StartDate))
            //    .ForMember(dest => dest.AcademicYearEndDate, opt => opt.MapFrom(src => src.AcademicYear.EndDate));

            //Mapper.CreateMap<Grade, Frontend.Areas.Administration.Models.GradeDetailsViewModel>()
            //    .ForMember(dest => dest.AcademicYearStartDate, opt => opt.MapFrom(src => src.AcademicYear.StartDate))
            //    .ForMember(dest => dest.AcademicYearEndDate, opt => opt.MapFrom(src => src.AcademicYear.EndDate))
            //    .ForMember(dest => dest.AcademicYearIsActive, opt => opt.MapFrom(src => src.AcademicYear.IsActive))
            //    .ForMember(dest => dest.SchoolClassesCount, opt => opt.MapFrom(src => src.SchoolClasses.Count()))
            //    .ForMember(
            //        dest => dest.SchoolClasses,
            //        opt => opt.MapFrom(src => src.SchoolClasses.OrderBy(sc => sc.ClassLetter)));

            //Mapper.CreateMap<Frontend.Areas.Administration.Models.GradeDetailsViewModel, Grade>();

            //Mapper.CreateMap<SchoolClass, Frontend.Areas.Administration.Models.SchoolClassListViewModel>()
            //    .ForMember(dest => dest.GradeYear, opt => opt.MapFrom(src => src.Grade.GradeYear))
            //    .ForMember(
            //        dest => dest.StudentsNumber,
            //        opt => opt.MapFrom(src => src.Students.Count(s => s.IsDeleted == false)))
            //    .ForMember(
            //        dest => dest.AcademicYearStartDate,
            //        opt => opt.MapFrom(src => src.Grade.AcademicYear.StartDate));

            //Mapper.CreateMap<SchoolClass, Frontend.Areas.Administration.Models.SchoolClassDetailsViewModel>()
            //    .ForMember(dest => dest.GradeYear, opt => opt.MapFrom(src => src.Grade.GradeYear))
            //    .ForMember(
            //        dest => dest.StudentsNumber,
            //        opt => opt.MapFrom(src => src.Students.Count(s => s.IsDeleted == false)))
            //    .ForMember(
            //        dest => dest.Students,
            //        opt => opt.MapFrom(src => src.Students.Where(s => s.IsDeleted == false)))
            //    .ForMember(dest => dest.AcademicYear, opt => opt.MapFrom(src => src.Grade.AcademicYear));

            //Mapper.CreateMap<SchoolClass, Frontend.Areas.Administration.Models.SchoolClassEditViewModel>()
            //    .ForMember(dest => dest.AcademicYearId, opt => opt.MapFrom(src => src.Grade.AcademicYearId));

            //Mapper.CreateMap<Frontend.Areas.Administration.Models.SchoolClassEditViewModel, SchoolClass>();

            //Mapper.CreateMap<SchoolClass, Frontend.Areas.Administration.Models.SchoolClassCreateSubmitModel>();

            //Mapper.CreateMap<Frontend.Areas.Administration.Models.SchoolClassCreateSubmitModel, SchoolClass>();

            //Mapper.CreateMap<SchoolClass, Frontend.Areas.Administration.Models.SchoolClassDeleteViewModel>()
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
        }
    }
}