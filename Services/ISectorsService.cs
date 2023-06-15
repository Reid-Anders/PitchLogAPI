using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.ResourceParameters;

namespace PitchLogAPI.Services
{
    public interface ISectorsService
    {
        public Task<SectorDTO> GetByID(int ID);

        public Task<PagedList<SectorDTO>> GetSectors(int areaID, SectorsResourceParameters parameters);

        public Task<SectorDTO> CreateArea(int areaID, SectorForCreationDTO sectorForCreation);

        public Task<bool> UpdateSector(int areaID, int ID, SectorForUpdateDTO sectorForUpdate);

        public Task<bool> PatchSector(int areaID, int ID, JsonPatchDocument<SectorForUpdateDTO> patchDocument, ControllerBase controller);

        public Task<bool> DeleteSector(int areaID, int ID);
    }
}
