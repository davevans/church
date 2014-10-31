namespace Church.Components.Account
{
    public interface IAuthenticationService
    {
        bool Authenticate(string username, string password);
    }
}
