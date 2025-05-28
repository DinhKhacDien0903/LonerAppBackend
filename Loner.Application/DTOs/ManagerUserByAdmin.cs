namespace Loner.Application.DTOs
{
    public class ManagerUserByAdmin
    {
    }

    public record GetAllUserRequest(PaginationRequest Request) : IRequest<Result<GetAllUserResponse>>;
    public record GetAllUserResponse(PaginatedResponse<UserIdentityDto> Users, int TotalUsers, int TotalBlockedUsers, int TotalReportedUsers);
}