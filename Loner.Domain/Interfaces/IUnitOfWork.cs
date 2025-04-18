namespace Loner.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository UserRepository { get; }
    IOtpRepository OtpRepository { get; }
    IRefreshTokenRepository RefreshTokenRepository { get; }
    ISwipeRepository SwipeRepository { get; }
    IMatchesRepository MatchesRepository { get; }
    IPhotoRepository PhotoRepository { get; }
    IInterestRepository InterestRepository { get; }
    IMessageRepository MessageRepository { get; }
    Task<int> CommitAsync();
}