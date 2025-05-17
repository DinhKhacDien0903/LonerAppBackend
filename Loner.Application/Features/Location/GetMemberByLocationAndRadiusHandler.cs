using System.Globalization;
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
            var longitude = MapHelper.ParseWithAutoSeparator(request.Longitude);
            var latitude = MapHelper.ParseWithAutoSeparator(request.Latitude);
            var validateResult = ValidateRequest(request);
            if (!validateResult.IsSuccess)
                return validateResult;
            var currentUser = await _uow.UserRepository.GetByIdAsync(request.UserId);
            if (currentUser == null)
                return Result<GetMemberByLocationAndRadiusResponse>.Failure("User not found");

            int countUser = await _uow.UserRepository.GetTotalUserCountAsync();
            var users = (await _uow.SwipeRepository.GetUnSwipedUsersAsync(request.UserId, 0, countUser)).Items;
            List<UserEntity> satisfyUsers = [];
            foreach (var user in users)
            {
                if (user.Id == request.UserId)
                    continue;
                var distance = MapHelper.GetDistance(latitude, longitude, user.Latitude, user.Longitude);
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
                Longitude = x.Longitude.ToString(CultureInfo.InvariantCulture),
                Latitude = x.Latitude.ToString(CultureInfo.InvariantCulture),
                Description = string.IsNullOrEmpty(x.University)
                ? $"{x.Address} - {MapHelper.GetDistance(currentUser.Latitude, currentUser.Longitude, x.Latitude, x.Longitude)} km"
                : $"{x.University} - {MapHelper.GetDistance(currentUser.Latitude, currentUser.Longitude, x.Latitude, x.Longitude)} km"
            })];

            return Result<GetMemberByLocationAndRadiusResponse>.Success(new GetMemberByLocationAndRadiusResponse(results));
        }

        private Result<GetMemberByLocationAndRadiusResponse> ValidateRequest(GetMemberByLocationAndRadiusRequest request)
        {
            var longitude = MapHelper.ParseWithAutoSeparator(request.Longitude);
            var latitude = MapHelper.ParseWithAutoSeparator(request.Latitude);
            if (string.IsNullOrEmpty(request.UserId))
                return Result<GetMemberByLocationAndRadiusResponse>.Failure("Invalid UserId");
            if (longitude < -180 || longitude > 180)
                return Result<GetMemberByLocationAndRadiusResponse>.Failure("Invalid Longitude");
            if (latitude < -90 || latitude > 90)
                return Result<GetMemberByLocationAndRadiusResponse>.Failure("Invalid Latitude");
            if (request.Radius < 0 || request.Radius > 100)
                return Result<GetMemberByLocationAndRadiusResponse>.Failure("Invalid Radius");
            return Result<GetMemberByLocationAndRadiusResponse>.Success(null);
        }
    }
}