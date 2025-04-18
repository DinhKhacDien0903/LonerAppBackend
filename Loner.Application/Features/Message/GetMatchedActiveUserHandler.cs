using Loner.Application.DTOs;
using static Loner.Application.DTOs.Message;

namespace Loner.Application.Features.Message;

public class GetMatchedActiveUserHandler : IRequestHandler<GetMatchedActiveUserRequest, Result<GetMatchedActiveUserResponse>>
{
    private readonly IUnitOfWork _uow;
    public GetMatchedActiveUserHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    public async Task<Result<GetMatchedActiveUserResponse>> Handle(GetMatchedActiveUserRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.PaginationRequest.UserId))
            return Result<GetMatchedActiveUserResponse>.Failure("Unauthorized");

        var matches = await _uow.MatchesRepository.GetMatchPagiatedAsync(request.PaginationRequest.UserId,
            request.PaginationRequest.ValidPageNumber, request.PaginationRequest.ValidPageSize);

        var activeThreshold = DateTime.UtcNow.AddMinutes(-5);
        var matchedUserIds = matches.Select(x => x.User1Id == request.PaginationRequest.UserId ? x.User2Id : x.User1Id);
        List<UserBasicDto> results = new ();
        foreach (var matchedUserId in matchedUserIds)
        {
            var user = await _uow.UserRepository.GetByIdAsync(matchedUserId);
            if (user != null && user.LastActive >= activeThreshold)
            {
                results.Add(new UserBasicDto
                {
                    Id = user.Id,
                    Username = user.UserName ?? "Dinh Khac Dien",
                    Age = user.Age,
                    AvatarUrl = user.AvatarUrl
                });
            }
        }

        return Result<GetMatchedActiveUserResponse>.Success(new GetMatchedActiveUserResponse(
            new PaginatedResponse<UserBasicDto>
            {
                Items = results,
                //TODO: Add api for get total records
                // TotalItems = await _uow.MatchesRepository.GetTotalRecordsAsync(request.PaginationRequest.UserId),
                TotalItems = results.Count,
                PageNumber = request.PaginationRequest.ValidPageNumber,
                PageSize = request.PaginationRequest.ValidPageSize
            }));
    }
}