using static Loner.Application.DTOs.Swipe;

namespace Loner.Application.Features.Swipe;

public class SwipeProfileHandler : IRequestHandler<SwipeRequest, Result<SwipeResponse>>
{
    private readonly IUnitOfWork _uow;
    public SwipeProfileHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public Task<Result<SwipeResponse>> Handle(SwipeRequest request, CancellationToken cancellationToken)
    {
            // if (string.IsNullOrEmpty(request.SwiperId))
            // {
            //     return Result<GetProfilesResponse>.Failure("Unauthorized");
            // }
            throw new Exception();
    }
}