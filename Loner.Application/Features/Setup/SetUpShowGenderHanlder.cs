using Loner.Application.DTOs;
using Loner.Domain.Entities;

namespace Loner.Application.Features.Setup
{
    public class SetUpShowGenderHanlder : IRequestHandler<SetUpShowGenderRequest, Result<SetUpResponse>>
    {
        private readonly IUnitOfWork _uow;
        public SetUpShowGenderHanlder(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<Result<SetUpResponse>> Handle(SetUpShowGenderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    return Result<SetUpResponse>.Failure("Invalid User or UserName");

                var preference = await _uow.PreferenceRepository.GetByUserId(request.UserId);
                if (preference == null)
                {
                    await InitPreference(request.UserId, request.ShowGender);
                }
                else
                {
                    preference.PreferenceGender = request.ShowGender;
                    _uow.PreferenceRepository.Update(preference);
                }

                await _uow.CommitAsync();

                return Result<SetUpResponse>.Success(new SetUpResponse(IsSuccess: true));
            }
            catch (Exception ex)
            {
                return Result<SetUpResponse>.Failure("Có lỗi khi thiết lập tài khoản " + ex.Message);
            }
        }

        private async Task InitPreference(string userId, bool genderShow)
        {
            var newPreference = new PreferenceEntity
            {
                Id = Guid.NewGuid().ToString(),
                PreferenceGender = genderShow,
                UserId = userId
            };

            await _uow.PreferenceRepository.AddAsync(newPreference);
        }
    }
}