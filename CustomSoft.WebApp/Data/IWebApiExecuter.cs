
namespace CustomSoft.WebApp.Server.Data
{
    public interface IWebApiExecuter
    {
        Task InvokeDelete(string relativeUrl);
        Task<T?> InvokeGet<T>(string relativeUrl);
        Task<T?> InvokePost<T>(string relativeUrl, T obj);
        Task InvokePut<T>(string relativeUrl, T obj);
        Task InvokePostWithFiles(string relativeUrl, IEnumerable<KeyValuePair<string, HttpContent>> formContents);
        Task<Stream> InvokeGetAsStream(string relativeUrl);
    }
}