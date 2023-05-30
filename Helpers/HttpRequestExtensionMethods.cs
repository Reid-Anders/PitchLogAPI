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

        public static string GetPathAndQuery(this HttpRequest request)
        {
            var pathAndQuery = request.Path;

            if(request.Query.Count > 0)
            {
               pathAndQuery += "?" + string.Join('&', request.Query.Select(kvPair => kvPair.Key + "=" + kvPair.Value));
            }

            return pathAndQuery;
        }
    }
}
