using Loner.Domain.Entities;
using static Loner.Application.DTOs.Swipe;

namespace Loner.Application.Features.Swipe;

public class SwipeProfileHandler : IRequestHandler<SwipeRequest, Result<SwipeResponse>>
{
    private readonly IUnitOfWork _uow;
    public SwipeProfileHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<SwipeResponse>> Handle(SwipeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(request.SwiperId))
                return Result<SwipeResponse>.Failure("Unauthorized");

            var swipedUser = await _uow.UserRepository.GetByIdAsync(request.SwipedId);
            if (swipedUser == null)
                return Result<SwipeResponse>.Failure("User not found");

            var existingSwipe = await _uow.SwipeRepository.GetSwipeAsync(request.SwiperId, request.SwipedId);
            if (existingSwipe != null)
                return Result<SwipeResponse>.Success(new SwipeResponse("Swipe recorded", false));
            //return Result<SwipeResponse>.Failure("Already swiped");

            var newSwipe = InitSwipe(request);
            await _uow.SwipeRepository.AddAsync(newSwipe);

            bool isMatch = false;
            if (request.Action == true)
            {
                var reverseSwipe = await _uow.SwipeRepository.GetSwipeAsync(request.SwipedId, request.SwiperId);
                if (reverseSwipe != null && reverseSwipe.Action == true)
                {
                    var newMatch = InitMatch(request);
                    await _uow.MatchesRepository.AddAsync(newMatch);
                    isMatch = true;
                }
            }

            await _uow.CommitAsync();

            return Result<SwipeResponse>.Success(new SwipeResponse(Message: isMatch ? "It's a match!" : "Swipe recorded", isMatch));
        }
        catch(Exception ex)
        {
            return Result<SwipeResponse>.Failure($"Exception occurred: {ex.Message}");
        }
    }

    private SwipeEntity InitSwipe(SwipeRequest request)
    {
        return new SwipeEntity
        {
            Id = Guid.NewGuid().ToString(),
            SwiperId = request.SwiperId,
            SwipedId = request.SwipedId,
            Action = request.Action,
            CreatedAt = DateTime.UtcNow
        };
    }

    private MatchesEntity InitMatch(SwipeRequest request)
    {
        return new MatchesEntity
        {
            Id = Guid.NewGuid().ToString(),
            User1Id = request.SwiperId,
            User2Id = request.SwipedId,
            CreatedAt = DateTime.UtcNow
        };
    }
}