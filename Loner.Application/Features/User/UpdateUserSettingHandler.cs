using Loner.Domain.Entities;
using static Loner.Application.DTOs.User;

namespace Loner.Application.Features.User
{
    public class UpdateUserSettingHandler : IRequestHandler<UpdateUserSettingRequest, Result<UpdateUserSettingResponse>>
    {
        private readonly IUnitOfWork _uow;
        public UpdateUserSettingHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<UpdateUserSettingResponse>> Handle(UpdateUserSettingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.EditRequest.UserId))
                    return Result<UpdateUserSettingResponse>.Failure("Invalid User");

                var user = await _uow.UserRepository.GetByIdAsync(request.EditRequest.UserId);
                if (user == null)
                    return Result<UpdateUserSettingResponse>.Failure("User not found");

                user.PhoneNumber = request.EditRequest.PhoneNumber;
                user.Email = request.EditRequest.Email;
                user.Address = request.EditRequest.Address;
                _uow.UserRepository.Update(user);

                var preference = await _uow.PreferenceRepository.GetByUserId(request.EditRequest.UserId);
                if (preference == null)
                    await InitPreference(request);
                else
                    UpdatePreference(preference, request);

                await _uow.CommitAsync();

                return Result<UpdateUserSettingResponse>.Success(new UpdateUserSettingResponse(IsSuccess: true));
            }
            catch (Exception ex)
            {
                return Result<UpdateUserSettingResponse>.Failure("Throw Exception " + ex.Message);
            }
        }

        private async Task InitPreference(UpdateUserSettingRequest request)
        {
            var newPreference = new PreferenceEntity
            {
                Id = Guid.NewGuid().ToString(),
                UserId = request.EditRequest.UserId,
                PreferenceGender = request.EditRequest.ShowGender,
                MinAge = request.EditRequest.MinAge,
                MaxAge = request.EditRequest.MaxAge
            };

            await _uow.PreferenceRepository.AddAsync(newPreference);
        }

        private void UpdatePreference(PreferenceEntity preference, UpdateUserSettingRequest request)
        {
            preference.PreferenceGender = request.EditRequest.ShowGender;
            preference.MinAge = request.EditRequest.MinAge;
            preference.MaxAge = request.EditRequest.MaxAge;
            _uow.PreferenceRepository.Update(preference);
        }
    }
}