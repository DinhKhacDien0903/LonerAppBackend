namespace Loner.Application.DTOs;

public class Profile
{
    public record GetProfilesRequest(PaginationRequest PaginationRequest) : IRequest<Result<GetProfilesResponse>>;
    public record GetProfilesResponse(UserBasicDto user);
}
