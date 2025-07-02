using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RepeaterCouncil.Web.Services
{
    public class QrzAuthResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public string SessionKey { get; set; }
        public string Callsign { get; set; }
        public string Email { get; set; }
    }

    public class QrzAuthService
    {
        private readonly HttpClient _httpClient;

        public QrzAuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<QrzAuthResult> LoginAsync(string callsign, string password)
        {
            var url = $"https://xmldata.qrz.com/xml/current/?username={callsign}&password={password}";
            var response = await _httpClient.GetStringAsync(url);

            var xml = XDocument.Parse(response);
            var error = xml.Root?.Element("Error")?.Value;
            if (!string.IsNullOrEmpty(error))
            {
                return new QrzAuthResult { Success = false, Error = error };
            }

            var sessionKey = xml.Root?.Element("Session")?.Element("Key")?.Value;
            var call = xml.Root?.Element("Session")?.Element("Callsign")?.Value;
            var email = xml.Root?.Element("Session")?.Element("Email")?.Value;

            return new QrzAuthResult
            {
                Success = true,
                SessionKey = sessionKey,
                Callsign = call,
                Email = email
            };
        }
    }
}
