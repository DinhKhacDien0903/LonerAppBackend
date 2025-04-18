using Loner.Application.DTOs;
using static Loner.Application.DTOs.Message;

namespace Loner.Application.Features.Message;

public class GetBasicUserMessageHandler : IRequestHandler<GetBasicUserMessageRequest, Result<GetBasicUserMessageResponse>>
{
    private readonly IUnitOfWork _uow;
    public GetBasicUserMessageHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<GetBasicUserMessageResponse>> Handle(GetBasicUserMessageRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.PaginationRequest.UserId))
            return Result<GetBasicUserMessageResponse>.Failure("Unauthorized");

        var matches = await _uow.MatchesRepository.GetMatchPagiatedAsync(request.PaginationRequest.UserId,
            request.PaginationRequest.ValidPageNumber, request.PaginationRequest.ValidPageSize);

        List<string> matchIds = [.. matches.Select(x => x.Id)];
        List<UserMessageBasicDto> results = new();
        foreach (var item in matchIds)
        {
            var currentMatch = await _uow.MatchesRepository.GetByIdAsync(item);
            var userId = currentMatch?.User1Id == request.PaginationRequest.UserId ? currentMatch.User2Id : currentMatch?.User1Id;
            var currentUser = await _uow.UserRepository.GetByIdAsync(userId ?? "");
            var lastMessage = await _uow.MessageRepository.GetLastMessageByMatchIdAsync(item);
            results.Add(new UserMessageBasicDto
            {
                UserId = userId ?? "",
                MatchId = item,
                UserName = currentUser?.UserName ?? "Dien",
                AvatarUrl = currentUser?.AvatarUrl ?? "https://res.cloudinary.com/de0werx80/image/upload/v1744905317/bbbb_edwkwg.jpg",
                LastMessage = lastMessage?.Content,
                IsCurrentUserSend = lastMessage?.SenderId == request.PaginationRequest.UserId,
                SendTime = lastMessage?.CreatedAt
            });
        }

        return Result<GetBasicUserMessageResponse>.Success(new GetBasicUserMessageResponse(
            new PaginatedResponse<UserMessageBasicDto>
            {
                //TODO: Add api for get total records
                // TotalItems = await _uow.MatchesRepository.GetTotalRecordsAsync(request.PaginationRequest.UserId),
                Items = results,
                TotalItems = results.Count,
                PageNumber = request.PaginationRequest.ValidPageNumber,
                PageSize = request.PaginationRequest.ValidPageSize
            }));
    }
}