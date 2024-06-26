﻿
namespace CustomSoft.WebApp.Server.Data
{
    public class WebApiExecuter : IWebApiExecuter
    {
        private const string ApiName = "CustomSoftApi";
        private readonly IHttpClientFactory _httpClientFactory;
        public WebApiExecuter(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            var httpClient = _httpClientFactory.CreateClient(ApiName);
            var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
            var response = await httpClient.SendAsync(request);

            await HandlePotentialError(response);

            return await response.Content.ReadFromJsonAsync<T>();

        }

        public async Task<T?> InvokePost<T>(string relativeUrl, T obj)
        {
            var httpClient = _httpClientFactory.CreateClient(ApiName);
            var response = await httpClient.PostAsJsonAsync(relativeUrl, obj);

            await HandlePotentialError(response);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task InvokePut<T>(string relativeUrl, T obj)
        {
            var httpClient = _httpClientFactory.CreateClient(ApiName);
            var response = await httpClient.PutAsJsonAsync(relativeUrl, obj);

            await HandlePotentialError(response);
        }

        public async Task InvokeDelete(string relativeUrl)
        {
            var httpClient = _httpClientFactory.CreateClient(ApiName);
            var response = await httpClient.DeleteAsync(relativeUrl);

            await HandlePotentialError(response);
        }

        public async Task InvokePostWithFiles(string relativeUrl, IEnumerable<KeyValuePair<string, HttpContent>> formContents)
        {
            var httpClient = _httpClientFactory.CreateClient(ApiName);
            using var formData = new MultipartFormDataContent();
            foreach (var content in formContents)
            {
                formData.Add(content.Value, content.Key);
            }

            var response = await httpClient.PostAsync(relativeUrl, formData);
            await HandlePotentialError(response);
        }

        private async Task HandlePotentialError(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                throw new WebApiException(errorJson);
            }
        }

        public async Task<Stream> InvokeGetAsStream(string relativeUrl)
        {
            var httpClient = _httpClientFactory.CreateClient(ApiName);
            var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
            var response = await httpClient.SendAsync(request);

            await HandlePotentialError(response);

            return await response.Content.ReadAsStreamAsync();
        }
    }
}
