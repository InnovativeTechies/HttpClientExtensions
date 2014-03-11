using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HttpClientExtensions.Test
{
    [TestClass]
    public class HttpMapperTest
    {
        [TestMethod]
        public async Task SucessfullyGetUrl()
        {
            var client = new HttpClient();
            var mapper = new Func<string, Task<string>>(s => Task.FromResult(s.Contains("html").ToString()));
            var res = await client.DownloadMappedItemAsync("http://www.google.com", mapper);

            Assert.AreEqual(true, res.IsSuccessfull);
        }

        [TestMethod]
        public async Task GetsCorrectStatusCode()
        {
            var client = new HttpClient();
            var mapper = new Func<string, Task<string>>(s => Task.FromResult(s.Contains("html").ToString()));
            var res = await client.DownloadMappedItemAsync("http://www.google.com", mapper);

            Assert.AreEqual(200, res.StatusCode);
        }

        [TestMethod]
        public async Task SuccessfullyDownloadsData()
        {
            var client = new HttpClient();
            var mapper = new Func<string, Task<string>>(s => Task.FromResult(s.Contains("html").ToString()));
            var res = await client.DownloadMappedItemAsync("http://www.google.com", mapper);

            Assert.AreEqual("True", res.Result);
        }

        [TestMethod]
        public async Task Handles404()
        {
            var client = new HttpClient();
            var mapper = new Func<string, Task<string>>(s => Task.FromResult(s.Contains("html").ToString()));
            var res = await client.DownloadMappedItemAsync("http://www.google.com/asfaghreh", mapper);

            Assert.AreEqual(false, res.IsSuccessfull);
            Assert.AreEqual(404, res.StatusCode);
        }

        [TestMethod]
        public async Task HandlesBadUrl()
        {
            var client = new HttpClient();
            var mapper = new Func<string, Task<string>>(s => Task.FromResult(s.Contains("html").ToString()));
            var res = await client.DownloadMappedItemAsync("dgdsgda.com", mapper);

            Assert.AreEqual(false, res.IsSuccessfull);
        }
    }
}
