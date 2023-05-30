using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PitchLogAPI.Model;

namespace PitchLogAPI.Services
{
    public abstract class BaseService
    {
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _contextAccessor;
        protected readonly ProblemDetailsFactory _problemDetailsFactory;
        protected readonly LinkGenerator _linkGenerator;

        public BaseService(IMapper mapper,
            IHttpContextAccessor contextAccessor,
            ProblemDetailsFactory problemDetailsFactory,
            LinkGenerator linkGenerator)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            _problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));
            _linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
        }

        public abstract void LinkResource(BaseDTO resource);

        public virtual void LinkResources(IEnumerable<BaseDTO> resources)
        {
            foreach(var resource in resources)
            {
                LinkResource(resource);
            }
        }
    }
}
