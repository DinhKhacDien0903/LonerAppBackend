using Loner.Application.DTOs;
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
            var matchedUserIds = matches.Select(x => x.User1Id == request.UserId ? x.User2Id : x.User1Id).ToList();

            var result = new List<UserBasicDto>();
            foreach(var item in matchedUserIds)
            {
                var userItem = await _uow.UserRepository.GetByIdAsync(item);
                if(userItem == null)
                    break;

                var user = new UserBasicDto
                {
                    Id = userItem.Id,
                    Username = userItem.UserName,
                    Age = userItem.Age,
                    AvatarUrl = userItem.AvatarUrl
                };

                result.Add(user);
            }

            return Result<GetMatchesResponse>.Success(new GetMatchesResponse(Matches: result));
        }
    }
}