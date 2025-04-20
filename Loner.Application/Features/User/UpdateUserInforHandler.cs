using Loner.Domain.Entities;
using static Loner.Application.DTOs.User;

namespace Loner.Application.Features.User
{
    public class UpdateUserInforHandler : IRequestHandler<UpdateUserInforRequest, Result<UpdateUserInforResponse>>
    {
        private readonly IUnitOfWork _uow;
        public UpdateUserInforHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<Result<UpdateUserInforResponse>> Handle(UpdateUserInforRequest request, CancellationToken cancellationToken)
        {
            var validateResult = ValidateRequest(request);
            if(!validateResult.IsSuccess)
                return validateResult;

            var user = await _uow.UserRepository.GetByIdAsync(request.EditRequest.UserId);
            if(user == null)
                return Result<UpdateUserInforResponse>.Failure("User not found");

            user.About = request.EditRequest.About;
            user.Gender = request.EditRequest.Gender;
            user.University = request.EditRequest.University;
            user.Work = request.EditRequest.Work;
            _uow.UserRepository.Update(user);
        }

        private Result<UpdateUserInforResponse> ValidateRequest(UpdateUserInforRequest request)
        {
            if (string.IsNullOrEmpty(request.EditRequest.UserId))
                return Result<UpdateUserInforResponse>.Failure("Invalid User");
            if (!request.EditRequest.Photos.Any())
                return Result<UpdateUserInforResponse>.Failure("Invalid Photos");
            if (!request.EditRequest.Interests.Any())
                return Result<UpdateUserInforResponse>.Failure("Invalid Interests");
            return Result<UpdateUserInforResponse>.Success(null);
        }

        private async Task UpdateUserPhotosAsync(string userId, IEnumerable<string> photos)
        {
            var photosOfuser = await _uow.PhotoRepository.GetPhotosByUserIdAsync(userId);
            foreach(var photo in photosOfuser)
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
        }
    }
}