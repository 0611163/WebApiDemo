using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    /// <summary>
    /// HttpClient工厂类
    /// </summary>
    public class HttpClientFactory
    {
        private static readonly IHttpClientFactory _httpClientFactory;

        static HttpClientFactory()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            ServiceProvider serviceProvider = serviceCollection.AddHttpClient().BuildServiceProvider();
            _httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        }

        public static HttpClient GetClient()
        {
            return _httpClientFactory.CreateClient();
        }
    }
}
