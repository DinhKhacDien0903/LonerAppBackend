using Loner.Application.DTOs;
using static Loner.Application.DTOs.Message;

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
        try
        {
            if (string.IsNullOrEmpty(request.PaginationRequest.UserId))
                return Result<SearchByNameResponse>.Failure("Unauthorized");
            if (string.IsNullOrEmpty(request.PaginationRequest.UserNameValue))
                return Result<SearchByNameResponse>.Failure("Value can not be empty");

            var matches = await _uow.MatchesRepository.GetMatchPagiatedAsync(request.PaginationRequest.UserId,
                request.PaginationRequest.ValidPageNumber, request.PaginationRequest.ValidPageSize, isFilterUnChatBlocked: false);

            List<UserBasicDto> results = new();

            foreach (var item in matches)
            {
                var userId = item?.User1Id == request.PaginationRequest.UserId ? item.User2Id : item?.User1Id;
                var currentUser = await _uow.UserRepository.GetUserContainNameAsync(userId ?? "", request.PaginationRequest.UserNameValue.Trim() ?? "");
                if (currentUser == null)
                    continue;
                results.Add(new UserBasicDto
                {
                    Id = userId ?? "",
                    MatchId = item?.Id ?? "",
                    Username = currentUser?.UserName ?? "Dien",
                    AvatarUrl = currentUser?.AvatarUrl ?? "https://res.cloudinary.com/de0werx80/image/upload/v1744905317/bbbb_edwkwg.jpg"
                });
            }

            return Result<SearchByNameResponse>.Success(new SearchByNameResponse(
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
        catch (Exception e)
        {
            return Result<SearchByNameResponse>.Failure("Accour error: " + e.Message);
        }
    }
}