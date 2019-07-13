using System.Net;
using Newtonsoft.Json;

namespace IntakeTracker.Infrastructure
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        public Response(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
