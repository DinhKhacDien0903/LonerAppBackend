namespace Loner.Application.DTOs;

public record SearchByNameRequest(PaginationRequest PaginationRequest) : IRequest<Result<SearchByNameResponse>>;
public record SearchByNameResponse(PaginatedResponse<UserBasicDto> User);