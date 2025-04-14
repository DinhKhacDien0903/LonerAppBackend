namespace Loner.Application.DTOs;

public class ProfileDetail
{
    public record GetProfileDetailRequest(string UserId) : IRequest<Result<GetProfileDetailResponse>>;
    public record GetProfileDetailResponse(UserIdentityDto UserDetail);
}