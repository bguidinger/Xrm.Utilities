namespace BGuidinger.Base
{
    public interface ITokenService
    {
        Token GetToken(string resource, string username, string password);
    }
}