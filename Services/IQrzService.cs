namespace RepeaterCouncil.Services
{
    public interface IQrzService
    {
        Task<QRZ.Response> Login(string username, string password);
        Task<QRZ.Response> Lookup(string callsign);
    }
}
