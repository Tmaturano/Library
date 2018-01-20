using AutoMapper;

namespace Library.Application.AutoMapper
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(config =>
            {
                config.AddProfile(new DomainToOutputDtoMappingProfile());
                config.AddProfile(new InputDtoToDomainMappingProfile());
            });
        }
    }
}
