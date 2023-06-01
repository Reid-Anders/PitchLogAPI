using PitchLogAPI.Model;

namespace PitchLogAPI.Services
{
    public interface ILinkFactory
    {
        public LinkDTO Get(string routeName, object routeValues);

        public LinkDTO Get(string routeName, object routeValues, string rel);

        public LinkDTO Post(string routeName, object routeValue);

        public LinkDTO Put(string routeName, object routeValue);

        public LinkDTO Patch(string routeName, object routeValue);

        public LinkDTO Delete(string routeName, object routeValue);
    }
}
