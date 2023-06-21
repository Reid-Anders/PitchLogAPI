﻿using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.ResourceParameters;
using PitchLogAPI.Services;

namespace PitchLogAPI.Controllers
{
    [Route("api/v1/Areas/{areaID}")]
    [ApiController]
    public class RoutesController : AreaBaseController
    {
        private readonly IRouteService _routesService;

        public RoutesController(IRouteService routesService,
            ILinkFactory linkFactory) : base(linkFactory)
        {
            _routesService = routesService ?? throw new ArgumentNullException(nameof(routesService));
        }

        [HttpGet("Routes", Name = nameof(GetAreaRoutes))]
        public async Task<IActionResult> GetAreaRoutes(int areaID, [FromQuery] RoutesResourceParameters parameters)
        {
            var routesToReturn = await _routesService.GetRoutes(areaID, parameters);
            return PrepareRouteCollectionResponse(routesToReturn, parameters);
        }

        [HttpGet("{sectorID}/Routes", Name = nameof(GetSectorRoutes))]
        public async Task<IActionResult> GetSectorRoutes(int areaID, int sectorID, [FromQuery] RoutesResourceParameters parameters)
        {
            var routesToReturn = await _routesService.GetRoutes(areaID, sectorID, parameters);
            return PrepareRouteCollectionResponse(routesToReturn, parameters);
        }

        private IActionResult PrepareRouteCollectionResponse(PagedList<RouteDTO> routes, RoutesResourceParameters parameters)
        {
            Response.AddPaginationHeaders(routes);

            LinkResources(routes);
            var links = LinkCollection(parameters);

            return Ok(new
            {
                resource = routes,
                links
            });
        }

        [HttpGet("{sectorID}/Routes/{ID}", Name = nameof(GetRouteByID))]
        public async Task<IActionResult> GetRouteByID(int areaID, int sectorID, int ID)
        {
            var routeToReturn = await _routesService.GetByID(areaID, sectorID, ID);

            LinkResource(routeToReturn);

            return Ok(routeToReturn);
        }

        [HttpPost("{sectorID}/Routes/{ID}", Name = nameof(CreateRoute))]
        public async Task<IActionResult> CreateRoute(int areaID, int sectorID, RouteForCreationDTO routeForCreation) 
        {
            var routeToReturn = await _routesService.CreateRoute(areaID, sectorID, routeForCreation);

            LinkResource(routeToReturn);

            return Ok(routeToReturn);
        }

        [HttpPut("{sectorID}/Routes/{ID}", Name = nameof(UpdateRoute))]
        public async Task<IActionResult> UpdateRoute(int areaID, int sectorID, int ID, RouteForUpdateDTO routeForUpdate)
        {
            await _routesService.UpdateRoute(areaID, sectorID, ID, routeForUpdate);
            return NoContent();
        }

        [HttpPatch("{sectorID}/Routes/{ID}", Name = nameof(PatchRoute))]
        public async Task<IActionResult> PatchRoute(int areaID, int sectorID, int ID, JsonPatchDocument<RouteForUpdateDTO> patchDocument)
        {
            await _routesService.PatchRoute(areaID, sectorID, ID, patchDocument, this);
            return NoContent();
        }

        [HttpDelete("{sectorID}/Routes/{ID}", Name = nameof(DeleteRoute))]
        public async Task<IActionResult> DeleteRoute(int areaID, int sectorID, int ID)
        {
            await _routesService.DeleteRoute(areaID, sectorID, ID);
            return NoContent();
        }
        
        protected override void LinkResource(BaseDTO dto)
        {
            if(dto is not RouteDTO routeDTO)
            {
                return;
            }

            var routeValues = new { areaID, sectorID, routeDTO.ID };
            dto.Links.Add(_linkFactory.Get(nameof(GetRouteByID), routeValues));
            dto.Links.Add(_linkFactory.Put(nameof(UpdateRoute), routeValues));
            dto.Links.Add(_linkFactory.Patch(nameof(PatchRoute), routeValues));
            dto.Links.Add(_linkFactory.Delete(nameof(DeleteRoute), routeValues));
            dto.Links.Add(_linkFactory.Get(nameof(SectorsController.GetSectorByID), new { areaID, ID = sectorID }, "sector"));
        }

        protected override IList<LinkDTO> LinkCollection(BaseResourceParameters parameters)
        {
            var links = base.LinkCollection(parameters);
            var allParameters = parameters.SplitAndAugment(
                KeyValuePair.Create<string, object>(nameof(areaID), areaID),
                KeyValuePair.Create<string, object>(nameof(sectorID), sectorID));

            if(sectorID == 0)
            {
                links.Add(_linkFactory.Get(nameof(GetAreaRoutes), allParameters));
            }
            else
            {
                links.Add(_linkFactory.Get(nameof(GetSectorRoutes), allParameters));
                links.Add(_linkFactory.Post(nameof(CreateRoute), allParameters));
            }

            return links;
        }
    }
}
