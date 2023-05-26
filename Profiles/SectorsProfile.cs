using AutoMapper;

namespace PitchLogAPI.Profiles
{
    public class SectorsProfile : Profile
    {
        public SectorsProfile()
        {
            CreateMap<PitchLogLib.Entities.Sector, Model.SectorDTO>()
                .ForMember(dto => dto.Aspect, options =>
                {
                    options.MapFrom(src => src.Aspect.ToString());
                });
            CreateMap<Model.SectorForCreationDTO, PitchLogLib.Entities.Sector>();
            CreateMap<Model.SectorForUpdateDTO, PitchLogLib.Entities.Sector>().ReverseMap();
        }
    }
}
