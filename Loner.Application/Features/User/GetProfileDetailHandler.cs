using Loner.Application.DTOs;
using static Loner.Application.DTOs.ProfileDetail;

namespace Loner.Application.Features.User;

public class GetProfileDetailHandler : IRequestHandler<GetProfileDetailRequest, Result<GetProfileDetailResponse>>
{
    private readonly IUnitOfWork _uow;
    public GetProfileDetailHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<GetProfileDetailResponse>> Handle(GetProfileDetailRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            return Result<GetProfileDetailResponse>.Failure("UserId not null or empty");

        var userDetail = await _uow.UserRepository.GetByIdAsync(request.UserId);
        if (userDetail == null)
            return Result<GetProfileDetailResponse>.Failure("User not found");

        var photos = await _uow.PhotoRepository.GetPhotosByUserIdAsync(request.UserId);
        var interests = await _uow.InterestRepository.GetInterestsByUserIdAsync(request.UserId);

        var result = new UserIdentityDto
        {
            Id = userDetail.Id,
            Email = userDetail.Email,
            UserName = userDetail.UserName,
            Age = userDetail.Age,
            AvatarUrl = userDetail.AvatarUrl,
            About = userDetail.About,
            Address = userDetail.Address,
            University = userDetail.University,
            Work = userDetail.Work,
            Gender = userDetail.Gender,
            IsActive = userDetail.IsActive,
            DateOfBirth = userDetail.DateOfBirth,
            Photos = photos.Select(x => x.Url).ToList(),
            Interests = interests.Select(x => x.Name).ToList(),
        };

        return Result<GetProfileDetailResponse>.Success(new GetProfileDetailResponse(UserDetail: result));
    }
}