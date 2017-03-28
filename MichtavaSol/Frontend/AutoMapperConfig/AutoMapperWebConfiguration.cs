namespace Frontend.AutoMapperConfig
{
    using AutoMapper;

    public static class AutoMapperWebConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(configuration =>
            {
                configuration.AddProfile<OrganizationProfile>();
            });
        }
    }
}