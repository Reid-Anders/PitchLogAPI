using Microsoft.AspNetCore.Mvc;

namespace PitchLogAPI.Helpers
{
    public class RestException : Exception
    {
        public ProblemDetails Details { get; private set; }

        public RestException(ProblemDetails details)
        {
            Details = details;
        }
    }
}
