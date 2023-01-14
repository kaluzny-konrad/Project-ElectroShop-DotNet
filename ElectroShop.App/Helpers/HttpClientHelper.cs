namespace ElectroShop.App.Helpers
{
    public interface IHttpClientHelper
    {
        Task<Stream> GetStreamAsync(string? requestUri);
        Task<HttpResponseMessage> PostAsync(string? requestUri, HttpContent? content);
        Task<HttpResponseMessage> PutAsync(string? requestUri, HttpContent? content);
        Task<HttpResponseMessage> DeleteAsync(string? requestUri);
    }

    public class HttpClientHelper : IHttpClientHelper
    {
        private readonly HttpClient _httpClient;
        public HttpClientHelper(HttpClient httpClient) =>
            _httpClient = httpClient;

        public Task<Stream> GetStreamAsync(string? requestUri)
        {
            return _httpClient.GetStreamAsync(requestUri);
        }

        public Task<HttpResponseMessage> PostAsync(string? requestUri, HttpContent? content)
        {
            return _httpClient.PostAsync(requestUri, content);
        }

        public Task<HttpResponseMessage> PutAsync(string? requestUri, HttpContent? content)
        {
            return _httpClient.PutAsync(requestUri, content);
        }
        public Task<HttpResponseMessage> DeleteAsync(string? requestUri)
        {
            return _httpClient.DeleteAsync(requestUri);
        }
    }
}
