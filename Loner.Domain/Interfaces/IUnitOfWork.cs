namespace Loner.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository UserRepository { get; }
    IOtpRepository OtpRepository { get; }
    IRefreshTokenRepository RefreshTokenRepository { get; }
    Task<int> CommitAsync();
}