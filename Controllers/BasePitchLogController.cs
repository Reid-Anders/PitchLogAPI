using Microsoft.AspNetCore.Mvc;
using PitchLogAPI.Model;
using PitchLogAPI.ResourceParameters;
using PitchLogAPI.Services;

namespace PitchLogAPI.Controllers
{
    public abstract class BasePitchLogController : ControllerBase
    {
        protected readonly ILinkFactory _linkFactory;

        public BasePitchLogController(ILinkFactory linkFactory)
        {
            _linkFactory = linkFactory ?? throw new ArgumentNullException(nameof(linkFactory));
        }

        protected virtual IList<LinkDTO> LinkCollection(BaseResourceParameters parameters)
        {
            return new List<LinkDTO>();
        }

        protected abstract Task<bool> LinkResource(BaseDTO dto);

        protected virtual async Task<bool> LinkResources(IEnumerable<BaseDTO> dtoList)
        {
            foreach(var dto in dtoList)
            {
                await LinkResource(dto);
            }

            return true;
        }

    }
}
