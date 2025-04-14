namespace Loner.Application.DTOs;

public class Swipe
{
    public record SwipeRequest(string SwiperId, string SwipedId, bool Action) : IRequest<Result<SwipeResponse>>;
    public record SwipeResponse(string Message, bool IsMatch);
}