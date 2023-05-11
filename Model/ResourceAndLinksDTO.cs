namespace PitchLogAPI.Model
{
    public class ResourceAndLinksDTO
    {
        public object Resource { get; private set; }

        public IList<LinkDTO> Links { get; private set; }

        public ResourceAndLinksDTO(object resource, IList<LinkDTO> links)
        {
            Resource = resource;
            Links = links;
        }
    }
}
