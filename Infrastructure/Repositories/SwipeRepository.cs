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
        return await _context.Matches.Where(x => x.User1Id == userId || x.User2Id == userId).ToListAsync();
    }

    public async Task<SwipeEntity?> GetSwipeAsync(string swiperId, string swipedId)
    {
        return await _context.Swipes.FirstOrDefaultAsync(x => x.SwiperId == swiperId && x.SwipedId == swipedId);
    }

    //Get 10 raw users until the user swiped
    //public async Task<PaginatedResponse<UserEntity>> GetUnSwipedUsersAsync(string userId, int pageNumber, int pageSize)
    //{
    //    var validPageNumber = Math.Max(1, pageNumber);
    //    var validPageSize = Math.Min(Math.Max(1, pageSize), 10);
    //    var userPreference = await _context.Preferences.Where(x => x.UserId == userId).FirstOrDefaultAsync();

    //    var swipedUsersIds = await _context.Swipes
    //        .Where(x => x.SwiperId == userId)
    //        .Select(x => x.SwipedId)
    //        .ToListAsync();
    //    var query = _context.Users
    //            .Where(x => x.Id != userId && !swipedUsersIds.Contains(x.Id));
    //    int totalItems = await query.CountAsync();
    //    //todo: filter follow interest, preference
    //    //var users = await _context.Users
    //    //    .Where(x => x.Id != userId
    //    //     && !swipedUsersIds.Contains(x.Id))
    //    //     //&& x.Gender == userPreference.PreferenceGender
    //    //     //&& x.Age >= userPreference.MinAge
    //    //     //&& x.Age <= userPreference.MaxAge)
    //    //    .Skip((validPageNumber - 1) * validPageSize)
    //    //    .Take(validPageSize)
    //    //    .Select(x => new UserEntity
    //    //    {
    //    //        Id = x.Id,
    //    //        UserName = x.UserName,
    //    //        Age = x.Age,
    //    //        AvatarUrl = x.AvatarUrl,
    //    //    })
    //    //    .ToListAsync();
    //    var users = await query
    //            .OrderBy(x => x.Id)
    //            .Skip((validPageNumber - 1) * validPageSize)
    //            .Take(validPageSize)
    //            .Select(x => new UserEntity
    //            {
    //                Id = x.Id,
    //                UserName = x.UserName,
    //                Age = x.Age,
    //                AvatarUrl = x.AvatarUrl,
    //            })
    //            .ToListAsync();

    //    return new PaginatedResponse<UserEntity>
    //    {
    //        Items = users,
    //        PageNumber = validPageNumber,
    //        PageSize = validPageSize,
    //        TotalItems = totalItems
    //    };
    //    //return users;
    //}

    public async Task<PaginatedResponse<UserEntity>> GetUnSwipedUsersAsync(string userId, int pageNumber, int pageSize)
    {
        var validPageNumber = Math.Max(1, pageNumber);
        var validPageSize = Math.Min(Math.Max(1, pageSize), 10);

        var userPreference = await _context.Preferences
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();

        var swipedUsersIds = await _context.Swipes
            .Where(x => x.SwiperId == userId)
            .Select(x => x.SwipedId)
            .ToListAsync();

        var query = _context.Users
            .Where(x => x.Id != userId && !swipedUsersIds.Contains(x.Id));

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
            {
                skipCount = Math.Max(0, totalItems - validPageSize);
            }

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
                })
                .ToListAsync();
        }

        return new PaginatedResponse<UserEntity>
        {
            Items = users,
            PageNumber = validPageNumber,
            PageSize = validPageSize,
            TotalItems = totalItems
        };
    }
}