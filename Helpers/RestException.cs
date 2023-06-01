namespace PitchLogAPI.Helpers
{
    public class RestException : Exception
    {
        public RestException(string? message) : base(message)
        {
        }
    }
}
