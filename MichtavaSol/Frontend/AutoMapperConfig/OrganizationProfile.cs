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
           

            Mapper.CreateMap<Frontend.Areas.Students.Models.AccountViewModels.RegisterViewModel, Student>();//create a map from 1 model to the other, so that later we won't have to do type: {x.name = y.name} 50 times. 


            //admin - edit my applicationuser attributes 
            Mapper.CreateMap<ApplicationUser, Frontend.Areas.Administration.Models.AccountDetailsEditModel>();

            Mapper.CreateMap<Frontend.Areas.Administration.Models.AccountDetailsEditModel, ApplicationUser>();


            //registery - admins non-applicationuser attributes
            Mapper.CreateMap<Frontend.Areas.Administration.Models.AdministratorRegisterSubmitModel, Administrator>();


            //admin  - edit an admins details
            Mapper.CreateMap<Administrator, Frontend.Areas.Administration.Models.AdministratorDetailsEditModel>()
                .ForMember(dest => dest.AccountDetailsEditModel, opt => opt.MapFrom(src => src.ApplicationUser));

            Mapper.CreateMap<Frontend.Areas.Administration.Models.AdministratorDetailsEditModel, Administrator>()
                .ForMember(dest => dest.ApplicationUser, opt => opt.MapFrom(src => src.AccountDetailsEditModel));


            //admin - view list of admins
            Mapper.CreateMap<Administrator, Frontend.Areas.Administration.Models.AdministratorListViewModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email));

            Mapper.CreateMap<Frontend.Areas.Administration.Models.AdministratorListViewModel, Administrator>();

            //admin - delete admin
            Mapper.CreateMap<Administrator, Frontend.Areas.Administration.Models.AdministratorDeleteSubmitModel>();

            ////admin - list all students
            //Mapper.CreateMap<Student, Frontend.Areas.Administration.Models.StudentListViewModel>()
            //    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
            //    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email));

            //Mapper.CreateMap<Frontend.Areas.Administration.Models.StudentListViewModel, Student>();

            ////admin - edit students details
            //Mapper.CreateMap<Student, Frontend.Areas.Administration.Models.StudentDetailsEditModel>();

            //Mapper.CreateMap<Frontend.Areas.Administration.Models.StudentDetailsEditModel, Student>();
        }
    }
}