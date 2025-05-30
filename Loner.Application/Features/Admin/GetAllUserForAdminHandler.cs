using Loner.Application.DTOs;
using Loner.Domain;
namespace Loner.Application.Features.Admin
{
    public class GetAllUserForAdminHandler : IRequestHandler<GetAllUserForAdminRequest, Result<GetAllUserForAdminResponse>>
    {
        private readonly IUnitOfWork _uow;
        public GetAllUserForAdminHandler(
            IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<Result<GetAllUserForAdminResponse>> Handle(GetAllUserForAdminRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    return Result<GetAllUserForAdminResponse>.Failure("Current user not found");

                var users = await _uow.UserRepository.GetAllUserByFilterAsync(
                    request.UserId ?? "",
                    request.UserName,
                    request.PhoneNumber,
                    request.Email,
                    request.PageNumber,
                    request.PageSize);

                PaginatedResponse<UserIdentityDto> result = new();
                result.PageSize = users.PageSize;
                result.TotalItems = users.TotalItems;
                List<UserIdentityDto> temps = new();
                foreach(var item in users.Items)
                {
                    temps.Add((await MapUserIdentityDto(item)));
                }

                result.Items = temps.ToList();
                return Result<GetAllUserForAdminResponse>.Success(new GetAllUserForAdminResponse(result));
            }
            catch (Exception ex)
            {
                return Result<GetAllUserForAdminResponse>.Failure($"Error: {ex.Message}");
            }
        }

        private async Task<UserIdentityDto> MapUserIdentityDto(UserEntity user)
        {
            var photos = await _uow.PhotoRepository.GetPhotosByUserIdAsync(user.Id);
            var interests = await _uow.InterestRepository.GetInterestsByUserIdAsync(user.Id);

            return new UserIdentityDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.FullName,
                Age = user.Age,
                AvatarUrl = user.AvatarUrl,
                About = user.About,
                Address = user.Address,
                University = user.University,
                Work = user.Work,
                Gender = user.Gender,
                IsActive = user.IsActive,
                DateOfBirth = user.DateOfBirth,
                IsDeleted = user.IsDeleted,
                CreatedAt = user.CreatedAt,
                Photos = photos.Select(x => x.Url).ToList(),
                Interests = interests.Select(x => x.Name).ToList(),
            };
        }
    }
}