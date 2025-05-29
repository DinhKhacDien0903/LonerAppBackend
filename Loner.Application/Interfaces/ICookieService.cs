namespace Loner.Application.Interfaces
{
    public interface ICookieService
    {
        void SaveTokenToCookieHttpOnly(string name, string token, int expiresMinutes);
        void RemoveTokenToCookieHttpOnly(string name);
    }
}