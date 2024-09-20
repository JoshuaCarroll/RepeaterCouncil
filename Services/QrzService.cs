using QRZ;
using System.Xml.Serialization;

namespace RepeaterCouncil.Services
{
    public class QrzService : IQrzService
    {
        private readonly HttpClient _httpClient;
        private readonly String _baseUrl = "https://xmldata.qrz.com/xml/current/";
        private string sessionKey;

        public QrzService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Response> Login(string username, string password)
        {
            var url = $"{_baseUrl}?username={username};password={password};agent=RepeaterCouncil.com";

            // Send HTTP request and get the response as a stream
            using (var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();  // Throw an exception if the status code is not success

                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    var serializer = new XmlSerializer(typeof(QRZ.Response));
                    QRZ.Response r = (QRZ.Response)serializer.Deserialize(stream);
                    sessionKey = r.Session.Key;
                    return r;
                }
            }
        }

        public async Task<Response> Lookup(string callsign)
        {
            var url = $"{_baseUrl}?s={sessionKey};callsign={callsign}";

            // Send HTTP request and get the response as a stream
            using (var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();  // Throw an exception if the status code is not success

                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    var serializer = new XmlSerializer(typeof(QRZ.Response));
                    return (QRZ.Response)serializer.Deserialize(stream);
                }
            }
        }

    }
}
