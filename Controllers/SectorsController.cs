﻿using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PitchLogAPI.Model;
using PitchLogAPI.ResourceParameters;
using PitchLogLib.Entities;
using PitchLogAPI.Helpers;
using Microsoft.Extensions.Options;
using PitchLogAPI.Services;

namespace PitchLogAPI.Controllers
{
    [Route("api/areas/{areaID}/sectors")]
    [ApiController]
    public class SectorsController : AreaBaseController
    {
        private readonly ISectorsService _sectorsService;

        public SectorsController(ISectorsService sectorsService,
            ILinkFactory linkFactory) : base(linkFactory)
        {
            _sectorsService = sectorsService ?? throw new ArgumentNullException(nameof(sectorsService));
        }

        [HttpGet("{ID}", Name = nameof(GetSectorByID))]
        public async Task<IActionResult> GetSectorByID(int areaID, int sectorID)
        {
            var sectorToReturn = await _sectorsService.GetByID(areaID, sectorID);

            await LinkResource(sectorToReturn);

            return Ok(sectorToReturn);
        }

        [HttpGet(Name = nameof(GetSectors))]
        public async Task<IActionResult> GetSectors(int areaID, [FromQuery] SectorsResourceParameters parameters)
        {
            var sectorsToReturn = await _sectorsService.GetSectors(areaID, parameters);

            Response.AddPaginationHeaders(sectorsToReturn);

            await LinkResources(sectorsToReturn);
            var links = LinkCollection(parameters);

            return Ok(new
            {
                resource = sectorsToReturn,
                links
            });
        }

        [HttpPost(Name = nameof(CreateSector))]
        public async Task<IActionResult> CreateSector(int areaID, SectorForCreationDTO sectorForCreation)
        {
            var sectorToReturn = await _sectorsService.CreateSector(areaID, sectorForCreation);

            await LinkResource(sectorToReturn);

            return CreatedAtRoute(nameof(GetSectorByID), new { areaID, sectorToReturn.ID }, sectorToReturn);
        }      

        [HttpPut("{ID}", Name = nameof(UpdateSector))]
        public async Task<IActionResult> UpdateSector(int areaID, int sectorID, SectorForUpdateDTO sectorForUpdate)
        {
            await _sectorsService.UpdateSector(areaID, sectorID, sectorForUpdate);
            return NoContent();
        }

        [HttpPatch("{ID}", Name = nameof(PatchSector))]
        public async Task<IActionResult> PatchSector(int areaID, int sectorID, JsonPatchDocument<SectorForUpdateDTO> patchDocument)
        {
            await _sectorsService.PatchSector(areaID, sectorID, patchDocument, this);
            return NoContent();
        }

        [HttpDelete("{ID}", Name = nameof(DeleteSector))]
        public async Task<IActionResult> DeleteSector(int areaID, int sectorID)
        {
            await _sectorsService.DeleteSector(areaID, sectorID);
            return NoContent();
        }

        protected override Task<bool> LinkResource(BaseDTO dto)
        {
            if(dto is not SectorDTO sectorDTO)
            {
                return Task.FromResult(false);
            }

            var routeValues = new { areaID, sectorDTO.ID };
            dto.Links.Add(_linkFactory.Get(nameof(GetSectorByID), routeValues));
            dto.Links.Add(_linkFactory.Put(nameof(UpdateSector), routeValues));
            dto.Links.Add(_linkFactory.Patch(nameof(PatchSector), routeValues));
            dto.Links.Add(_linkFactory.Delete(nameof(DeleteSector), routeValues));
            dto.Links.Add(_linkFactory.Get(nameof(AreasController.GetAreaByID), new { ID = areaID }, "area"));

            return Task.FromResult(true);
        }

        protected override IList<LinkDTO> LinkCollection(BaseResourceParameters parameters)
        {
            var links = base.LinkCollection(parameters);
            var allParameters = parameters.SplitAndAugment(KeyValuePair.Create<string, object>(nameof(areaID), areaID));

            links.Add(_linkFactory.Get(nameof(GetSectors), allParameters));
            links.Add(_linkFactory.Post(nameof(CreateSector), new { areaID }));

            return links;
        }
    }
}
