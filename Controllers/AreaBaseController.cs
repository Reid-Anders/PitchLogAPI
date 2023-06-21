using PitchLogAPI.Services;

namespace PitchLogAPI.Controllers
{
    public abstract class AreaBaseController : BasePitchLogController
    {
        protected int areaID
        {
            get
            {
                if (_areaID == null)
                {
                    Request.RouteValues.TryGetValue("areaID", out var value);
                    int.TryParse((string?)value, out int areaID);

                    _areaID = areaID;
                }

                return (int) _areaID;
            }
        }
        private int? _areaID = null;

        protected int sectorID
        {
            get
            {
                if (_sectorID == null)
                {
                    Request.RouteValues.TryGetValue("sectorID", out var value);
                    int.TryParse((string?)value, out int sectorID);

                    _sectorID = sectorID;
                }

                return (int)_sectorID;
            }
        }
        private int? _sectorID = null;

        public AreaBaseController(ILinkFactory linkFactory) : base(linkFactory)
        {

        }
    }
}
