using AutoMapper;

namespace PitchLogAPI.Profiles
{
    public class RoutesProfile : Profile
    {
        public RoutesProfile()
        {
            CreateMap<PitchLogLib.Entities.Route, PitchLogAPI.Model.RouteDTO>();
            CreateMap<PitchLogAPI.Model.RouteForCreationDTO, PitchLogLib.Entities.Route>();
            CreateMap<PitchLogAPI.Model.RouteForUpdateDTO, PitchLogLib.Entities.Route>()
                .ReverseMap();
        }
    }
}
