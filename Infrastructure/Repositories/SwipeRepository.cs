using Loner.Domain.Common;
using Loner.Domain.Entities;

namespace Infrastructure.Repositories;

public class SwipeRepository : BaseRepository<SwipeEntity>, ISwipeRepository
{
    public SwipeRepository(LonerDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MatchesEntity>> GetMatchesAsync(string userId)
    {
        return await _context.Matche.Where(x => x.User1Id == userId || x.User2Id == userId).ToListAsync();
    }

    public async Task<SwipeEntity?> GetSwipeAsync(string swiperId, string swipedId)
    {
        return await _context.Swipe.FirstOrDefaultAsync(x => x.SwiperId == swiperId && x.SwipedId == swipedId);
    }

    public async Task<PaginatedResponse<UserEntity>> GetUnSwipedUsersAsync(string userId, int pageNumber, int pageSize = 30)
    {
        var validPageNumber = Math.Max(1, pageNumber);
        var validPageSize = Math.Min(Math.Max(1, pageSize), 1000);

        var userPreference = await _context.Preference
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();

        //var swipedUsersIds = await _context.Swipe
        //    .Where(x => x.SwiperId == userId)
        //    .Select(x => x.SwipedId)
        //    .ToListAsync();

        var matchUserIds = await _context.Matche
            .Where(x => x.User1Id == userId || x.User2Id == userId)
            .Select(x => x.User1Id == userId ? x.User2Id : x.User1Id)
            .ToListAsync();

        var query = _context.Users
            .Where(x => !x.IsDeleted 
             && x.Id != userId 
             //&& !swipedUsersIds.Contains(x.Id) 
             && !matchUserIds.Contains(x.Id));

        //if (userPreference != null)
        //{
        //    query = query.Where(x => x.Gender == userPreference.PreferenceGender
        //        && x.Age >= userPreference.MinAge
        //        && x.Age <= userPreference.MaxAge);
        //}

        //if (!string.IsNullOrWhiteSpace(searchKey))
        //{
        //    searchKey = searchKey.ToLower();
        //    query = query.Where(x => x.UserName.ToLower().Contains(searchKey) ||
        //                            x.PhoneNumber.ToLower().Contains(searchKey));
        //}

        var totalItems = await query.CountAsync();

        var skipCount = (validPageNumber - 1) * validPageSize;

        var users = new List<UserEntity>();
        if (totalItems > 0)
        {
            if (skipCount >= totalItems)
                skipCount = 0;

            users = await query
                .OrderBy(x => x.Id)
                .Skip(skipCount)
                .Take(validPageSize)
                .Select(x => new UserEntity
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Age = x.Age,
                    AvatarUrl = x.AvatarUrl,
                    Longitude = x.Longitude,
                    Latitude = x.Latitude,
                    University = x.University,
                    Address = x.Address
                })
                .ToListAsync();
        }

        return new PaginatedResponse<UserEntity>
        {
            Items = users,
            PageNumber = validPageNumber,
            PageSize = validPageSize,
            TotalItems = totalItems,
        };
    }
}