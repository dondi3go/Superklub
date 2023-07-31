using System.Threading.Tasks;

namespace Superklub
{
    public class HttpResponse
    {
        public int Code { get; }
        public string Text { get; }

        public HttpResponse(int code, string text)
        {
            this.Code = code;
            this.Text = text;
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
    /// </summary>
    public interface IHttpClient
    {
        /// <summary>
        /// Performs an HTTP POST request 
        /// Return HTTP code + HTTP response payload
        /// HTTP code is 0 if the server cannot be reached 
        /// </summary>
        Task<HttpResponse> PostAsync(string jsonString);

        /// <summary>
        /// Performs an HTTP GET request
        /// Return HTTPCode + HTTP response payload
        /// HTTP code is 0 if the server cannot be reached
        /// </summary>
        Task<HttpResponse> GetAsync();
    }
}
