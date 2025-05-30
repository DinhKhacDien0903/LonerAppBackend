namespace Loner.Application.DTOs
{
    public record GetAllUserForAdminRequest : IRequest<Result<GetAllUserForAdminResponse>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 30;
        public int ValidPageNumber => Math.Max(1, PageNumber);
        public int ValidPageSize => Math.Max(1, Math.Min(100, PageSize));
        public string UserId { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }

    public record GetAllUserForAdminResponse(PaginatedResponse<UserIdentityDto> Data);
}