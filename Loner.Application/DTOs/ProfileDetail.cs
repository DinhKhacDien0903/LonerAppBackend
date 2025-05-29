namespace Loner.Application.DTOs;

public class ProfileDetail
{
    public record GetProfileDetailRequest(string UserId) : IRequest<Result<GetProfileDetailResponse>>;
    public class GetProfileDetailForAdminRequest : IRequest<Result<GetProfileDetailResponse>>
    {
        public string? UserId { get; set; }
    }
    public record GetProfileDetailResponse(UserIdentityDto UserDetail);
}