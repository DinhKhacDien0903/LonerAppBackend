namespace Loner.Application.DTOs;

public class Message
{
    public record GetMatchedActiveUserRequest(PaginationRequest PaginationRequest) : IRequest<Result<GetMatchedActiveUserResponse>>;
    public record GetMatchedActiveUserResponse(PaginatedResponse<UserBasicDto> Matches);
    public record GetBasicUserMessageRequest(PaginationRequest PaginationRequest) : IRequest<Result<GetBasicUserMessageResponse>>;
    public record GetBasicUserMessageResponse(PaginatedResponse<UserMessageBasicDto> UserMessages);
}