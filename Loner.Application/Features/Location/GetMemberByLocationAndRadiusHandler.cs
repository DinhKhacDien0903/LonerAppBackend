using Loner.Application.DTOs;
using Loner.Application.Helpers;
using Loner.Domain;
using static Loner.Application.DTOs.Location;

namespace Loner.Application.Features.Location
{
    public class GetMemberByLocationAndRadiusHandler
        : IRequestHandler<GetMemberByLocationAndRadiusRequest, Result<GetMemberByLocationAndRadiusResponse>>
    {
        private readonly IUnitOfWork _uow;
        public GetMemberByLocationAndRadiusHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<GetMemberByLocationAndRadiusResponse>> Handle
            (GetMemberByLocationAndRadiusRequest request, CancellationToken cancellationToken)
        {
            var validateResult = ValidateRequest(request);
            if(!validateResult.IsSuccess)
                return validateResult;

            var users = await _uow.UserRepository.GetAllAsync();
            List<UserEntity> satisfyUsers = [];
            foreach (var user in users)
            {
                if (user.Id == request.UserId)
                    continue;
                var distance = MapHelper.GetDistance(request.Latitude, request.Longtitude, user.Latitude, user.Longitude);
                if (distance <= request.Radius)
                {
                    satisfyUsers.Add(user);
                }
            }

            List<UserLocationDto> results = [.. satisfyUsers.Select(x => new UserLocationDto
            {
                UserId = x.Id,
                UserName = x.UserName ?? "Dinh Khac Dien",
                AvatarUrl = x.AvatarUrl,
                Description = string.IsNullOrEmpty(x.University)
                ? $"{x.Address} - {MapHelper.GetDistance(request.Latitude, request.Longtitude, x.Latitude, x.Longitude)}"
                : $"{x.University} - {MapHelper.GetDistance(request.Latitude, request.Longtitude, x.Latitude, x.Longitude)}"
            })];

            return Result<GetMemberByLocationAndRadiusResponse>.Success(new GetMemberByLocationAndRadiusResponse(results));
        }

        private Result<GetMemberByLocationAndRadiusResponse> ValidateRequest(GetMemberByLocationAndRadiusRequest request)
        {
            if (string.IsNullOrEmpty(request.UserId))
                return Result<GetMemberByLocationAndRadiusResponse>.Failure("Invalid UserId");
            if (request.Longtitude < -180 || request.Longtitude > 180)
                return Result<GetMemberByLocationAndRadiusResponse>.Failure("Invalid Longtitude");
            if (request.Latitude < -90 || request.Latitude > 90)
                return Result<GetMemberByLocationAndRadiusResponse>.Failure("Invalid Latitude");
            if(request.Radius < 0 || request.Radius > 100)
                return Result<GetMemberByLocationAndRadiusResponse>.Failure("Invalid Radius");
            return Result<GetMemberByLocationAndRadiusResponse>.Success(null);
        }
    }
}