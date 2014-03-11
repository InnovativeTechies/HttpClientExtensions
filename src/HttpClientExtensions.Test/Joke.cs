using System.Collections.Generic;

namespace HttpClientExtensions.Test
{
    public class Joke
    {
        public string type { get; set; }
        public Value value { get; set; }
    }

    public class Value
    {
        public int id { get; set; }
        public string joke { get; set; }
        public List<string> categories { get; set; }
    }
}