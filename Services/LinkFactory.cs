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

        public LinkDTO Post(string routeName, object routeValues)
        {
            return new LinkDTO(_generator.GetPathByName(routeName, routeValues), "create", "POST");
        }

        public LinkDTO Put(string routeName, object routeValues)
        {
            return new LinkDTO(_generator.GetPathByName(routeName, routeValues), "update_full", "PUT");
        }

        public LinkDTO Patch(string routeName, object routeValues)
        {
            return new LinkDTO(_generator.GetPathByName(routeName, routeValues), "update_partial", "PATCH");
        }

        public LinkDTO Delete(string routeName, object routeValues)
        {
            return new LinkDTO(_generator.GetPathByName(routeName, routeValues), "delete", "DELETE");
        }
    }
}
