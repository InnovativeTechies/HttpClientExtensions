A small extension for the HttpClient.

Usage for json:
var client = new HttpClient();
var res = await client.DownloadJsonAsync<Joke>("http://api.icndb.com/jokes/random");