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
            try
            {
                var validateResult = ValidateRequest(request);
                if (!validateResult.IsSuccess)
                    return validateResult;

                var user = await _uow.UserRepository.GetByIdAsync(request.EditRequest.UserId);
                if (user == null)
                    return Result<UpdateUserInforResponse>.Failure("User not found");

                user.About = request.EditRequest.About;
                user.Gender = request.EditRequest.Gender;
                user.University = request.EditRequest.University;
                user.Work = request.EditRequest.Work;
                _uow.UserRepository.Update(user);
                await UpdateUserPhotosAsync(request.EditRequest.UserId, request.EditRequest.Photos);
                await UpdateUserInterestAsync(request.EditRequest.UserId, request.EditRequest.Interests);

                await _uow.CommitAsync();

                return Result<UpdateUserInforResponse>.Success(new UpdateUserInforResponse(IsSuccess: true));
            }
            catch (Exception ex)
            {
                return Result<UpdateUserInforResponse>.Failure("Throw Exception " + ex.Message);
            }
        }

        private Result<UpdateUserInforResponse> ValidateRequest(UpdateUserInforRequest request)
        {
            if (string.IsNullOrEmpty(request.EditRequest.UserId))
                return Result<UpdateUserInforResponse>.Failure("Invalid User");
            if (!request.EditRequest.Photos.Any())
                return Result<UpdateUserInforResponse>.Failure("Invalid Photos");
            if (request.EditRequest.Photos.Count() < 2)
                return Result<UpdateUserInforResponse>.Failure("Photos must have at least 2 items.");
            if (!request.EditRequest.Interests.Any())
                return Result<UpdateUserInforResponse>.Failure("Invalid Interests");
            if (request.EditRequest.Interests.Count() < 5)
                return Result<UpdateUserInforResponse>.Failure("Interests must have at least 5 items.");
            return Result<UpdateUserInforResponse>.Success(null);
        }

        private async Task UpdateUserPhotosAsync(string userId, IEnumerable<string> photos)
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
        }

        private async Task UpdateUserInterestAsync(string userId, IEnumerable<string> interests)
        {
            var userIterests = await _uow.InterestRepository.GetUserInterestsByUserIdAsync(userId);
            foreach (var item in userIterests)
            {
                _uow.InterestRepository.DeleteUserInterest(item);
            }

            foreach (var interest in interests)
            {
                var newUserInterest = new User_InterestEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    InterestId = _uow.InterestRepository.GetIdByNameAsync(interest).Result
                };

                await _uow.InterestRepository.AddUserInterestAsync(newUserInterest);
            }
        }
    }
}