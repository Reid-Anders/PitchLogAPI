using Microsoft.AspNetCore.Mvc;
using PitchLogAPI.Model;
using PitchLogAPI.Repositories;

namespace PitchLogAPI.Controllers
{
    public class BasePitchLogController : ControllerBase
    {
        protected virtual void AddLinksToResource(BaseDTO dto)
        {
            //implementation optional
        }

        protected virtual void AddLinksToResources(IEnumerable<BaseDTO> dtoList)
        {
            foreach(var dto in dtoList)
            {
                AddLinksToResource(dto);
            }
        }

        protected virtual void AddLinksToResourceCollection(IEnumerable<BaseDTO> dtoList)
        {
            //implementation optional
        }
    }
}
