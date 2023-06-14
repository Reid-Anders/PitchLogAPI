using AutoMapper;
using PitchLogAPI.Helpers;

namespace PitchLogAPI.Profiles
{
    public class MiscProfile : Profile
    {
        public MiscProfile()
        {
            CreateMap(typeof(PagedList<>), typeof(PagedList<>))
                .ConvertUsing(typeof(PagedListConverter<,>));
        }
    }
}
