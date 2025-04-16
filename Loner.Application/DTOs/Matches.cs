namespace Loner.Application.DTOs;

public class Matches
{
    public record GetMatchesRequest(string UserId) : IRequest<Result<GetMatchesResponse>>;
    public record GetMatchesResponse(IEnumerable<UserBasicDto> Matches);
}