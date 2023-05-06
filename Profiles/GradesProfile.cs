using AutoMapper;

namespace PitchLogAPI.Profiles
{
    public class GradesProfile : Profile
    {
        public GradesProfile()
        {
            CreateMap<PitchLogLib.Entities.Grade, PitchLogAPI.Model.GradeDTO>()
                .ForMember(dto => dto.Type,
                options =>
                {
                    options.MapFrom(src => src.Type.ToString());
                });
        }
    }
}
