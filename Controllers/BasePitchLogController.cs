using Microsoft.AspNetCore.Mvc;
using PitchLogAPI.Model;
using PitchLogAPI.Repositories;

namespace PitchLogAPI.Controllers
{
    public class BasePitchLogController : ControllerBase
    {
        protected virtual void LinkResource(BaseDTO dto)
        {
            //implementation optional
        }

        protected virtual void LinkResources(IEnumerable<BaseDTO> dtoList)
        {
            foreach(var dto in dtoList)
            {
                LinkResource(dto);
            }
        }

        protected virtual void LinkCollection(IEnumerable<BaseDTO> dtoList)
        {
            //implementation optional
        }
    }
}
