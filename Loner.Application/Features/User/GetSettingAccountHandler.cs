using Loner.Application.DTOs;

namespace Loner.Application.Features.User;

public class GetSettingAccountHandler : IRequestHandler<GetSettingAccountRequest, Result<GetSettingAccountResponse>>
{
    private readonly IUnitOfWork _uow;
    public GetSettingAccountHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    public async Task<Result<GetSettingAccountResponse>> Handle(GetSettingAccountRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            return Result<GetSettingAccountResponse>.Failure("UserId not null or empty");

        var userDetail = await _uow.UserRepository.GetByIdAsync(request.UserId);
        if (userDetail == null)
            return Result<GetSettingAccountResponse>.Failure("User not found");
        var preferenUser = await _uow.PreferenceRepository.GetByUserId(request.UserId);
        if (preferenUser == null)
            return Result<GetSettingAccountResponse>.Failure("Preference not found");

        var result = new SettingAccountResponse
        {
            PhoneNumber = userDetail.PhoneNumber,
            Email = userDetail.Email,
            Address = userDetail.Address,
            ShowGender = preferenUser.PreferenceGender,
            MinAge = preferenUser.MinAge,
            MaxAge = preferenUser.MaxAge
        };

        return Result<GetSettingAccountResponse>.Success(new GetSettingAccountResponse(SettingAccount: result));
    }
}