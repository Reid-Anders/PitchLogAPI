using AutoMapper;

namespace PitchLogAPI.Profiles
{
    public class AreasProfile : Profile
    {
        public AreasProfile()
        {
            CreateMap<Model.AreaForCreationDTO, PitchLogLib.Entities.Area>();
            CreateMap<PitchLogLib.Entities.Area, Model.AreaDTO>();
        }
    }
}
