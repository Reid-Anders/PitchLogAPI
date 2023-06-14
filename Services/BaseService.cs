using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PitchLogAPI.Model;
using System.ComponentModel.DataAnnotations;

namespace PitchLogAPI.Services
{
    public abstract class BaseService
    {
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _contextAccessor;
        protected readonly ProblemDetailsFactory _problemDetailsFactory;

        public BaseService(IMapper mapper,
            IHttpContextAccessor contextAccessor,
            ProblemDetailsFactory problemDetailsFactory)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            _problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));
        }

        public bool TryValidateModel(object model, out IList<ValidationResult> results)
        {
            var context = new ValidationContext(model);
            results = new List<ValidationResult>();

            return Validator.TryValidateObject(model, context, results);
        }
    }
}
