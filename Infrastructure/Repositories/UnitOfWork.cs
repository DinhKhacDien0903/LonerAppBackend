namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly LonerDbContext _context;
    private IUserRepository? _users;
    private IOtpRepository? _otp;
    private ISwipeRepository? _swipe;
    private IMatchesRepository? _match;
    private IRefreshTokenRepository? _refresh;
    public IUserRepository UserRepository => _users ??= new UserRepository(_context);
    public IOtpRepository OtpRepository => _otp ??= new OtpRepository(_context);
    public IRefreshTokenRepository RefreshTokenRepository => _refresh ??= new RefreshTokenRepository(_context);
    public ISwipeRepository SwipeRepository => _swipe ??= new SwipeRepository(_context);
    public IMatchesRepository MatchesRepository => _match ??= new MatchesRepository(_context);

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