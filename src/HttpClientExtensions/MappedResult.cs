namespace System.Net.Http
{
    /// <summary>
    /// Wrapper class around the result
    /// </summary>
    /// <typeparam name="T">Result type</typeparam>
    public class MappedResult<T> where T : class
    {
        public readonly int StatusCode;
        public string ReasonPhrase { get; private set; }
        public readonly T Result;
        public bool IsSuccessfull { get; private set; }

        private MappedResult(T result, int statusCode)
        {
            StatusCode = statusCode;
            Result = result;
        }

        internal static MappedResult<T> CreateUnsuccessfull(HttpStatusCode statusCode, string reasonPhrase)
        {
            return CreateUnsuccessfull((int)statusCode, reasonPhrase);
        }

        internal static MappedResult<T> CreateUnsuccessfull(int statusCode, string reasonPhrase)
        {
            var mappedResult = new MappedResult<T>(null, statusCode)
            {
                ReasonPhrase = reasonPhrase,
                IsSuccessfull = false
            };
            return mappedResult;
        }

        internal static MappedResult<T> CreateSuccessfull(T result, HttpStatusCode statusCode)
        {
            var mappedResult = new MappedResult<T>(result, (int)statusCode) { IsSuccessfull = true };
            return mappedResult;
        }

        public override string ToString()
        {
            return string.Format("IsSuccessfully: {0}, StatusCode: {1}, ReasonPhrase: {2}, HasResult: {3}",
                IsSuccessfull, StatusCode, ReasonPhrase, Result != null);
        }
    }
}