using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HttpClientExtensions.Test
{
    [TestClass]
    public class JsonMapperTest
    {
        [TestMethod]
        public async Task SucessfullyGetUrl()
        {
            var client = new HttpClient();
            var res = await client.DownloadJsonAsync<Joke>("http://api.icndb.com/jokes/random");

            Assert.AreEqual(true, res.IsSuccessfull);
        }

        [TestMethod]
        public async Task GetsCorrectStatusCode()
        {
            var client = new HttpClient();
            var res = await client.DownloadJsonAsync<Joke>("http://api.icndb.com/jokes/random");

            Assert.AreEqual(200, res.StatusCode);
        }

        [TestMethod]
        public async Task SuccessfullyDownloadsData()
        {
            var client = new HttpClient();
            var res = await client.DownloadJsonAsync<Joke>("http://api.icndb.com/jokes/random");

            Assert.IsInstanceOfType(res.Result, typeof(Joke));
        }

        [TestMethod]
        public async Task Handles404()
        {
            var client = new HttpClient();
            var res = await client.DownloadJsonAsync<Joke>("http://www.google.com/asfaghreh");

            Assert.AreEqual(false, res.IsSuccessfull);
            Assert.AreEqual(404, res.StatusCode);
        }

        [TestMethod]
        public async Task HandlesBadUrl()
        {
            var client = new HttpClient();
            var res = await client.DownloadJsonAsync<Joke>("dgdsgda.com");

            Assert.AreEqual(false, res.IsSuccessfull);
        }

        [TestMethod]
        public async Task ReplacesCorrect()
        {
            var client = new HttpClient();
            var res = await client.DownloadJsonAsync<Joke>("http://api.icndb.com/jokes/random", new Dictionary<string, string> { { "Chuck", "Kalle" } });

            Assert.IsTrue(res.Result.value.joke.Contains("Kalle"));
        }
    }
}
