using static Loner.Application.DTOs.Matches;

namespace Loner.Application.Features.Matches
{
    public class GetMatchesRequestHandler : IRequestHandler<GetMatchesRequest, Result<GetMatchesResponse>>
    {
        private readonly IUnitOfWork _uow;
        public GetMatchesRequestHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<GetMatchesResponse>> Handle(GetMatchesRequest request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.UserId))
                return Result<GetMatchesResponse>.Failure("Unauthorized");
            var matches= await _uow.SwipeRepository.GetMatchesAsync(request.UserId);
            var matchesId = matches.Select(x => x.User1Id == request.UserId ? x.User2Id : x.User1Id).ToList();
        }
    }
}