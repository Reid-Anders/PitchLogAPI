using Microsoft.AspNetCore.Mvc;
using PitchLogAPI.Model;

namespace PitchLogAPI.Controllers
{
    public class BasePitchLogController : ControllerBase
    {
        protected virtual void AddLinksToResource(LinkedDTO dto)
        {
            //implementation optional
        }

        protected virtual void AddLinksToResources(IEnumerable<LinkedDTO> dtoList)
        {
            foreach(var dto in dtoList)
            {
                AddLinksToResource(dto);
            }
        }
    }
}
