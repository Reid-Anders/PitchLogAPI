namespace PitchLogAPI.Helpers
{
    public static class HttpRequestExtensionMethods
    {
        public static string GetAbsoluteUri(this HttpRequest Request)
        {
            return Request.Scheme + "://" + 
                Request.Host.Value + 
                Request.Path + "?" + 
                string.Join('&', Request.Query.Select(kvPair => kvPair.Key + "=" + kvPair.Value));
        }
    }
}
