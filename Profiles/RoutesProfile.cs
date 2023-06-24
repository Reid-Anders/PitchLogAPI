using AutoMapper;

namespace PitchLogAPI.Profiles
{
    public class RoutesProfile : Profile
    {
        public RoutesProfile()
        {
            CreateMap<PitchLogLib.Entities.Route, PitchLogAPI.Model.RouteDTO>()
                .ForMember(dto => dto.Length, options =>
                {
                    options.MapFrom(route => route.Length.Value);
                });
            CreateMap<PitchLogAPI.Model.RouteForCreationDTO, PitchLogLib.Entities.Route>()
                .ForMember(route => route.Length, options =>
                {
                    options.MapFrom(dto =>
                        dto.Length != null && dto.Length > 0 ?
                            new PitchLogLib.Entities.Length() { Value = (int)dto.Length, Units = PitchLogLib.LengthUnits.Meters } :
                            null);
                });
            CreateMap<PitchLogAPI.Model.RouteForUpdateDTO, PitchLogLib.Entities.Route>()
                .ReverseMap();
        }
    }
}
