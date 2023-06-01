using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.ResourceParameters;
using PitchLogAPI.Repositories;
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

            LinkResources(areasToReturn);

            var links = LinkCollection(parameters);
            links.Add(new LinkDTO(Url.Link(nameof(GetAreas), parameters), "self", "GET"));
            
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

            return Ok(areaToReturn);
        }

        //[HttpPost(Name = nameof(CreateArea))]
        //public async Task<IActionResult> CreateArea(AreaForCreationDTO areaForCreation)
        //{
        //    if(await _areasRepository.Exists(areaForCreation.Name))
        //    {
        //        return BadRequest(_problemDetailsFactory.CreateProblemDetails(
        //            HttpContext, statusCode: 400, detail: $"Area with the name {areaForCreation.Name} already exists."));
        //    }

        //    var areaToCreate = _mapper.Map<Area>(areaForCreation);
           
        //    _areasRepository.Create(areaToCreate);
        //    await _areasRepository.SaveChanges();

        //    var areaToReturn = _mapper.Map<AreaDTO>(areaToCreate);
        //    AddLinksToResource(areaToReturn);

        //    return CreatedAtRoute(nameof(GetAreaByID), new { areaToCreate.ID }, areaToReturn);
        //}

        //[HttpPut("{ID}", Name = nameof(UpdateAreaFull))]
        //public async Task<IActionResult> UpdateAreaFull(int ID, AreaForUpdateDTO areaForUpdate)
        //{
        //    var area = await _areasRepository.GetByID(ID);

        //    if(area == null)
        //    {
        //        return NotFound();
        //    }

        //    _mapper.Map(areaForUpdate, area);
        //    await _areasRepository.SaveChanges();

        //    return NoContent();
        //}

        //[HttpPatch("{ID}", Name = nameof(UpdateAreaPartial))]
        //public async Task<IActionResult> UpdateAreaPartial(int ID, JsonPatchDocument<AreaForUpdateDTO> patchDoc)
        //{
        //    var area = await _areasRepository.GetByID(ID);

        //    if(area == null)
        //    {
        //        return NotFound();
        //    }

        //    var areaToPatch = _mapper.Map<AreaForUpdateDTO>(area);
        //    patchDoc.ApplyTo(areaToPatch);

        //    if(!TryValidateModel(areaToPatch))
        //    {
        //        return HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>()
        //            .Value.InvalidModelStateResponseFactory(ControllerContext);
        //    }

        //    _mapper.Map(areaToPatch, area);
        //    await _areasRepository.SaveChanges();

        //    return NoContent();
        //}

        //[HttpDelete("{ID}", Name = nameof(DeleteArea))]
        //public async Task<IActionResult> DeleteArea(int ID)
        //{
        //    var areaToDelete = await _areasRepository.GetByID(ID);

        //    if(areaToDelete == null)
        //    {
        //        return NotFound();
        //    }

        //    _areasRepository.Delete(areaToDelete);
        //    await _areasRepository.SaveChanges();

        //    return NoContent();
        //}

        protected override void LinkResource(BaseDTO dto)
        {
            if(dto is not AreaDTO area)
            {
                return;
            }

            var id = new { area.ID };
            area.Links.Add(new LinkDTO(Url.Link(nameof(GetAreaByID), id), "self", "GET"));
            //area.Links.Add(new LinkDTO(Url.Link(nameof(CreateArea), new { }), "create", "POST"));
            //area.Links.Add(new LinkDTO(Url.Link(nameof(UpdateAreaFull), id), "update", "PUT"));
            //area.Links.Add(new LinkDTO(Url.Link(nameof(UpdateAreaPartial), id), "update_partial", "PATCH"));
            //area.Links.Add(new LinkDTO(Url.Link(nameof(DeleteArea), id), "delete", "DELETE"));
            area.Links.Add(new LinkDTO(Url.Link(nameof(SectorsController.GetSectors), new { areaID = area.ID }), "sectors", "GET"));
        }

        protected override IList<LinkDTO> LinkCollection(BaseResourceParameters parameters)
        {
            var links = new List<LinkDTO>();

            if(parameters is AreasResourceParameters areasParams)
            {
                li
            }

            return links;
        }
    }
}
