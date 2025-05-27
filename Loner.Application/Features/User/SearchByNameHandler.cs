using Loner.Application.DTOs;

namespace Loner.Application.Features.User;

public class SearchByNameHandler : IRequestHandler<SearchByNameRequest, Result<SearchByNameResponse>>
{
    private readonly IUnitOfWork _uow;
    public SearchByNameHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    public async Task<Result<SearchByNameResponse>> Handle(SearchByNameRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.PaginationRequest.UserId))
            return Result<SearchByNameResponse>.Failure("Unauthorized");
        if (string.IsNullOrEmpty(request.PaginationRequest.UserNameValue))
            return Result<SearchByNameResponse>.Failure("Value can not be empty");

        var matches = await _uow.MatchesRepository.GetMatchPagiatedAsync(request.PaginationRequest.UserId,
            request.PaginationRequest.ValidPageNumber, request.PaginationRequest.ValidPageSize, isFilterUnChatBlocked: false);

        List<string> matchIds = [.. matches.Select(x => x.Id)];
        List<UserBasicDto> results = new();

        // foreach (var item in matches)
        // {
        //     // var currentMatch = await _uow.MatchesRepository.GetByIdAsync(item);
        //     var userId = item?.User1Id == request.PaginationRequest.UserId ? item.User2Id : item?.User1Id;
        //     var currentUser = await _uow.UserRepository.GetUserByNameAsync(userId ?? "");
        //     if (currentUser == null)
        //         continue;
        //     results.Add(new UserBasicDto
        //     {
        //         Id = userId ?? "",
        //         MatchId = item?.Id ?? "",
        //         Username = currentUser?.UserName ?? "Dien",
        //         AvatarUrl = currentUser?.AvatarUrl ?? "https://res.cloudinary.com/de0werx80/image/upload/v1744905317/bbbb_edwkwg.jpg"
        //     });
        // }

        return Result<SearchByNameResponse>.Failure("Value can not be empty");
    }
}