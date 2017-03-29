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

         
        }
    }
}