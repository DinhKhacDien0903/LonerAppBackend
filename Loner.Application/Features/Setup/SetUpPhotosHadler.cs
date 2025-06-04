using Loner.Application.DTOs;
using Loner.Domain.Entities;

namespace Loner.Application.Features.Setup
{
    public class SetUpPhotosHadler : IRequestHandler<SetUpPhotosRequest, Result<SetUpResponse>>
    {
        private readonly IUnitOfWork _uow;
        public SetUpPhotosHadler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<SetUpResponse>> Handle(SetUpPhotosRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    return Result<SetUpResponse>.Failure("Invalid User or UserName");

                bool isSuccess = await UpdateUserPhotosAsync(request.UserId, request.Photos);

                await _uow.CommitAsync();

                return isSuccess ? Result<SetUpResponse>.Success(new SetUpResponse(IsSuccess: true)) :
                                  Result<SetUpResponse>.Failure("Failed to update photos");
            }
            catch (Exception ex)
            {
                return Result<SetUpResponse>.Failure("Có lỗi khi thiết lập tài khoản " + ex.Message);
            }
        }

        private async Task<bool> UpdateUserPhotosAsync(string userId, IEnumerable<string> photos)
        {
            try
            {
                var photosOfuser = await _uow.PhotoRepository.GetPhotosByUserIdAsync(userId);
                foreach (var photo in photosOfuser)
                {
                    await _uow.PhotoRepository.Delete(photo.Id);
                }

                foreach (var photo in photos)
                {
                    var newPhoto = new PhotoEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = userId,
                        Url = photo
                    };

                    await _uow.PhotoRepository.AddAsync(newPhoto);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}