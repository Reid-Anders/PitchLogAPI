using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.ResourceParameters;
using PitchLogLib.Entities;
using PitchLogAPI.Services;

namespace PitchLogAPI.Controllers
{
    [Route("api/Areas")]
    [ApiController]
    public class AreasController : BasePitchLogController
    {
        private readonly IAreasService _areasService;

        public AreasController(IAreasService areasService,
            IMapper mapper,
            ProblemDetailsFactory problemDetailsFactory,
            ILinkFactory linkFactory) : base(linkFactory)
        {
            _areasService = areasService ?? throw new ArgumentNullException(nameof(areasService));
        }

        [HttpGet(Name = nameof(GetAreas))]
        public async Task<IActionResult> GetAreas([FromQuery] AreasResourceParameters parameters)
        {
            var areasToReturn = await _areasService.GetAreas(parameters);
            Response.AddPaginationHeaders(areasToReturn, Request.GetAbsoluteUri());

            await LinkResources(areasToReturn);
            var links = LinkCollection(parameters);
            
            return Ok(new
            {
                resource = areasToReturn,
                links
            });
        }

        [HttpGet("{ID}", Name = nameof(GetAreaByID))]
        public async Task<IActionResult> GetAreaByID(int ID)
        {
            var areaToReturn = await _areasService.GetByID(ID);

            await LinkResource(areaToReturn);

            return Ok(areaToReturn);
        }

        [HttpPost(Name = nameof(CreateArea))]
        public async Task<IActionResult> CreateArea(AreaForCreationDTO areaForCreation)
        {
            var areaToReturn = await _areasService.CreateArea(areaForCreation);

            await LinkResource(areaToReturn);

            return CreatedAtRoute(nameof(GetAreaByID), new { areaToReturn.ID }, areaToReturn);
        }

        [HttpPut("{ID}", Name = nameof(UpdateArea))]
        public async Task<IActionResult> UpdateArea(int ID, AreaForUpdateDTO areaForUpdate)
        {
            await _areasService.UpdateArea(ID, areaForUpdate);
            return NoContent();
        }

        [HttpPatch("{ID}", Name = nameof(PatchArea))]
        public async Task<IActionResult> PatchArea(int ID, JsonPatchDocument<AreaForUpdateDTO> patchDocument)
        {
            await _areasService.PatchArea(ID, patchDocument, this);
            return NoContent();
        }

        [HttpDelete("{ID}", Name = nameof(DeleteArea))]
        public async Task<IActionResult> DeleteArea(int ID)
        {
            await _areasService.DeleteArea(ID);
            return NoContent();
        }

        protected override async Task<bool> LinkResource(BaseDTO dto)
        {
            if(dto is not AreaDTO area)
            {
                return false;
            }

            var id = new { area.ID };
            area.Links.Add(_linkFactory.Get(nameof(GetAreaByID), id));
            area.Links.Add(_linkFactory.Put(nameof(UpdateArea), id));
            area.Links.Add(_linkFactory.Patch(nameof(PatchArea), id));

            bool anySectors = await _areasService.AnySectors(area.ID);
            bool anyRoutes = await _areasService.AnyRoutes(area.ID);

            if (!anySectors && !anyRoutes)
            {
                area.Links.Add(_linkFactory.Delete(nameof(DeleteArea), id));
            }

            area.Links.Add(_linkFactory.Get(nameof(SectorsController.GetSectors), new { areaID = area.ID }, "sectors"));

            if (anyRoutes)
            {
                area.Links.Add(_linkFactory.Get(nameof(RoutesController.GetAreaRoutes), new { areaID = area.ID }, "routes"));
            }

            return true;
        }

        protected override IList<LinkDTO> LinkCollection(BaseResourceParameters parameters)
        {
            var links = base.LinkCollection(parameters);

            links.Add(_linkFactory.Get(nameof(GetAreas), parameters));
            links.Add(_linkFactory.Post(nameof(CreateArea), new { }));

            return links;
        }
    }
}
