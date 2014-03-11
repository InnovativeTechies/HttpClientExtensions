using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace System.Net.Http
{
    /// <summary>
    /// Extension to enable download and parsing Json
    /// </summary>
    public static class JsonClientExtension
    {
        /// <summary>
        /// Downloads url and parses the json result into provided type
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="self">The http client that will download the data</param>
        /// <param name="url">The url you want to download</param>
        /// <param name="replacementStrings">A dictionnary of strings if you want to replace something in the json</param>
        /// <returns>A result object with status codes and the parsed json</returns>        
        public static async Task<MappedResult<T>> DownloadJsonAsync<T>(this HttpClient self, string url,
            Dictionary<string, string> replacementStrings = null) where T : class
        {
            try
            {
                return await self.DownloadJsonAsync<T>(new Uri(url, UriKind.Absolute), replacementStrings);
            }
            catch (Exception ex)
            {
                return MappedResult<T>.CreateUnsuccessfull(-1, ex.Message);
            }
        }

        /// <summary>
        /// Downloads url and parses the json result into provided type
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="self">The http client that will download the data</param>
        /// <param name="url">The url you want to download</param>
        /// /// <param name="replacementStrings">A dictionnary of strings if you want to replace something in the json</param>
        /// <returns>A result object with status codes and the parsed json</returns>
        public static async Task<MappedResult<T>> DownloadJsonAsync<T>(this HttpClient self, Uri url,
            Dictionary<string, string> replacementStrings = null) where T : class
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
            if (replacementStrings != null)           
                content = replacementStrings.Aggregate(content,
                    (current, replacementString) => current.Replace(replacementString.Key, replacementString.Value));            

            var result = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(content));
            return MappedResult<T>.CreateSuccessfull(result, response.StatusCode);
        }
    }
}