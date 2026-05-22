namespace WebCoffee.BackendServer.Helpers
{
    public interface ICurrentUserService
    {
        string UserName { get; }
        string Role { get; }
        string MaNV { get; }
        bool IsAuthenticated { get; }
    }
}