namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly LonerDbContext _context;
    private IUserRepository? _users;
    private IOtpRepository? _otp;
    private ISwipeRepository? _swipe;
    private IMatchesRepository? _match;
    private IRefreshTokenRepository? _refresh;
    private IPhotoRepository? _photo;
    private IInterestRepository? _interest;
    private IMessageRepository? _message;
    public IUserRepository UserRepository => _users ??= new UserRepository(_context);
    public IOtpRepository OtpRepository => _otp ??= new OtpRepository(_context);
    public IRefreshTokenRepository RefreshTokenRepository => _refresh ??= new RefreshTokenRepository(_context);
    public ISwipeRepository SwipeRepository => _swipe ??= new SwipeRepository(_context);
    public IMatchesRepository MatchesRepository => _match ??= new MatchesRepository(_context);
    public IPhotoRepository PhotoRepository => _photo ??= new PhotoRepository(_context);
    public IInterestRepository InterestRepository => _interest ??= new InterestRepository(_context);
    public IMessageRepository MessageRepository => _message ??= new MessageRepository(_context);

    public UnitOfWork(LonerDbContext context)
    {
        _context = context;
    }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}