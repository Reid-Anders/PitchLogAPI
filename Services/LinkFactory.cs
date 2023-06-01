using PitchLogAPI.Model;

namespace PitchLogAPI.Services
{
    public class LinkFactory : ILinkFactory
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly LinkGenerator _generator;

        public LinkFactory(IHttpContextAccessor accessor,
            LinkGenerator generator)
        {
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
            _generator = generator ?? throw new ArgumentNullException(nameof(generator));
        }

        public LinkDTO Get(string routeName, object routeValues)
        {
            return Get(routeName, routeValues, "self");
        }

        public LinkDTO Get(string routeName, object routeValues, string rel)
        {
            return new LinkDTO(_generator.GetPathByName(routeName, routeValues), rel, "GET");
        }

        public LinkDTO Post(string routeName, object routeValue)
        {
            throw new NotImplementedException();
        }

        public LinkDTO Put(string routeName, object routeValue)
        {
            throw new NotImplementedException();
        }

        public LinkDTO Patch(string routeName, object routeValue)
        {
            throw new NotImplementedException();
        }

        public LinkDTO Delete(string routeName, object routeValue)
        {
            throw new NotImplementedException();
        }
    }
}
