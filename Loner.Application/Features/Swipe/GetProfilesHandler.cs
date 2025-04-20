using Loner.Application.DTOs;
using static Loner.Application.DTOs.Profile;

namespace Loner.Application.Features.Swipe
{
    public class GetProfilesHandler : IRequestHandler<GetProfilesRequest, Result<GetProfilesResponse>>
    {
        private readonly IUnitOfWork _uow;
        public GetProfilesHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<GetProfilesResponse>> Handle(GetProfilesRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.PaginationRequest.UserId))
                return Result<GetProfilesResponse>.Failure("Unauthorized");

            var users = await _uow.SwipeRepository.GetUnSwipedUsersAsync(request.PaginationRequest.UserId,
                                                                        request.PaginationRequest.ValidPageNumber,
                                                                        request.PaginationRequest.ValidPageSize);
            var totalItems = await _uow.UserRepository.GetTotalUserCountAsync();

            var basicUser = users.Select(x => new UserBasicDto
            {
                Id = x.Id,
                Age = x.Age,
                AvatarUrl = x.AvatarUrl,
                Username = x.UserName,
            }).ToList();

            var paginationResponse = new PaginatedResponse<UserBasicDto>
            {
                Items = basicUser,
                TotalItems = totalItems,
                PageNumber = request.PaginationRequest.ValidPageNumber,
                PageSize = request.PaginationRequest.ValidPageSize
            };

            return Result<GetProfilesResponse>.Success(new GetProfilesResponse(paginationResponse));
        }
    }
}