using System.Threading.Tasks;
using System.Collections.Generic;

namespace Superklub
{
    /// <summary>
    /// Response from an HTTP request
    /// </summary>
    public class HttpResponse
    {
        /// <summary>
        /// HTTP Code
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// HTTP payload (usually json)
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// A specific HTTP header value
        /// The returned value is matches CustomResponseHeader.Key
        /// </summary>
        public string CustomHeaderValue { get; }

        public HttpResponse(int code, string text, string headerValue = "")
        {
            this.Code = code;
            this.Text = text;
            this.CustomHeaderValue = headerValue;
        }
    }

    /// <summary>
    /// A request can contain additional headers.
    /// This class contains a pair of key / value to add to request headers
    /// </summary>
    public class CustomRequestHeader
    {
        public string Key { get; }
        public string Value { get; }

        public CustomRequestHeader(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        public bool IsEmpty()
        {
            return (Key == "");
        }
    }

    /// <summary>
    /// The response to a request also contains headers.
    /// The value of a specific header (identified by its key) can be added to the HTTP response
    /// </summary>
    public class CustomResponseHeader
    {
        public string Key { get; }

        public CustomResponseHeader(string key)
        {
            this.Key = key;
        }

        public bool IsEmpty()
        {
            return (Key == "");
        }
    }

    /// <summary>
    /// SupersynkClient needs to perform HTTP requests.
    /// 
    /// This interface allows different implementations
    /// depending on the target dotnet implementation.
    /// 
    /// For example, Unity and MS.Net  not 
    /// have a common way to perform HTTP requests.
    /// 
    /// Additional headers can be added to the request.
    /// One value in the response headers can be returned.
    /// </summary>
    public interface IHttpClient
    {
        /// <summary>
        /// Performs an HTTP POST request 
        /// Return HTTP code + HTTP response payload
        /// HTTP code is 0 if the server cannot be reached 
        /// </summary>
        Task<HttpResponse> PostAsync(
            string url, 
            string jsonString,
            List<CustomRequestHeader>? customRequestHeaders = null,
            CustomResponseHeader? customResponseHeader = null
            );

        /// <summary>
        /// Performs an HTTP GET request
        /// Return HTTPCode + HTTP response payload
        /// HTTP code is 0 if the server cannot be reached
        /// </summary>
        Task<HttpResponse> GetAsync(
            string url,
            List<CustomRequestHeader>? customRequestHeaders = null,
            CustomResponseHeader? customResponseHeader = null
            );
    }
}
