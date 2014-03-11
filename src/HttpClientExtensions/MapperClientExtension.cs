using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class MapperClientExtension
    {
        /// <summary>
        /// Downloads url and parses the html result into provided type
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="self">The http client that will download the data</param>
        /// <param name="url">The url you want to download</param>
        /// <param name="mapperFunc">A func that will parse the html into your type</param>
        /// <returns>A result object with status codes and the parsed html</returns>
        public static async Task<MappedResult<T>> DownloadMappedItemAsync<T>(this HttpClient self, string url,
            Func<string, Task<T>> mapperFunc) where T : class
        {
            try
            {
                return await self.DownloadMappedItemAsync(new Uri(url, UriKind.Absolute), mapperFunc);
            }
            catch (Exception ex)
            {
                return MappedResult<T>.CreateUnsuccessfull(-1, ex.Message);
            }
        }

        /// <summary>
        /// Downloads url and parses the html result into provided type
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="self">The http client that will download the data</param>
        /// <param name="url">The url you want to download</param>
        /// <param name="mapperFunc">A func that will parse the html into your type</param>
        /// <returns>A result object with status codes and the parsed html</returns>
        public static async Task<MappedResult<T>> DownloadMappedItemAsync<T>(this HttpClient self, Uri url,
            Func<string, Task<T>> mapperFunc) where T : class
        {
            HttpResponseMessage response;
            try
            {
                response = await self.GetAsync(url);
            }
            catch (Exception ex)
            {
                return MappedResult<T>.CreateUnsuccessfull(-1, ex.Message);
            }

            if (!response.IsSuccessStatusCode)
            {
                return MappedResult<T>.CreateUnsuccessfull(response.StatusCode, response.ReasonPhrase);
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = await mapperFunc(content);
            return MappedResult<T>.CreateSuccessfull(result, response.StatusCode);
        }
    }
}