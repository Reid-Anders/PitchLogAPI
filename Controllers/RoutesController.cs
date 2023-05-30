using Microsoft.AspNetCore.Mvc;
using PitchLogAPI.ResourceParameters;

namespace PitchLogAPI.Controllers
{
    [Route("api/v1/Areas/{areaID}")]
    [ApiController]
    public class RoutesController : BasePitchLogController
    {
        [HttpGet("Routes", Name = nameof(GetAreaRoutes))]
        public async Task<IActionResult> GetAreaRoutes(int areaID, RoutesResourceParameters parameters)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{sectorID}/Routes", Name = nameof(GetSectorRoutes))]
        public async Task<IActionResult> GetSectorRoutes(int areaID, int sectorID, RoutesResourceParameters parameters)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{sectorID}/Routes/{ID}")]
        public async Task<IActionResult> GetRoute(int areaId, int sectorID, int routeID)
        {
            throw new NotImplementedException();
        }
    }
}
