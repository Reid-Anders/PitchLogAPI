using Microsoft.AspNetCore.Mvc;
using PitchLogAPI.Model;
using PitchLogAPI.Repositories;
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

        protected abstract IList<LinkDTO> LinkCollection(BaseResourceParameters parameters);

        protected abstract void LinkResource(BaseDTO dto);

        protected virtual void LinkResources(IEnumerable<BaseDTO> dtoList)
        {
            foreach(var dto in dtoList)
            {
                LinkResource(dto);
            }
        }

    }
}
