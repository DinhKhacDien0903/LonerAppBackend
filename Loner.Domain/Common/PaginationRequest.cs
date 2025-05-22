namespace Loner.Domain.Common;

public partial class PaginationRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 30;
    public int ValidPageNumber => Math.Max(1, PageNumber);
    public int ValidPageSize => Math.Max(1, Math.Min(100, PageSize));
}

public partial class PaginationRequest
{
    public string? UserId { get; set; }
}
public partial class PaginationRequest
{
    public string? MatchId { get; set; }
}

public partial class PaginationRequest
{
    public bool IsMessageOfChatBot { get; set; }
}