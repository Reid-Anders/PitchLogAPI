using AutoMapper;
using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.Repositories;

namespace PitchLogAPI.Services
{
    public class AreasService : IAreasService
    {
        private readonly IAreasRepository _areasRepository;
        private readonly IMapper _mapper;

        public AreasService(IAreasRepository areasRepository, IMapper mapper)
        { 
            _areasRepository = areasRepository ?? throw new ArgumentNullException(nameof(areasRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<AreaDTO> GetByID(int ID)
        {
            var area = await _areasRepository.GetByID(ID);

            if(area == null)
            {
                throw new ResourceNotFoundException($"Area with id {ID} not found.");
            }

            var areaToReturn = _mapper.Map<AreaDTO>(area);
            return areaToReturn;
        }
    }
}
