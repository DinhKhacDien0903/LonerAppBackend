using static Loner.Application.DTOs.Location;

namespace Loner.Application.Features.Location
{
    public class UpdateLocationHandler : IRequestHandler<UpdateLocationRequest, Result<UpdateLocationResponse>>
    {
        private readonly IUnitOfWork _uow;
        public UpdateLocationHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<UpdateLocationResponse>> Handle(UpdateLocationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validateResult = ValidateRequest(request);
                if (!validateResult.IsSuccess)
                    return validateResult;

                var user = await _uow.UserRepository.GetByIdAsync(request.UserId);
                if(user == null)
                    return Result<UpdateLocationResponse>.Failure("User not found");

                user.Longitude = Math.Round(request.Longtitude, 4);
                user.Latitude = Math.Round(request.Latitude, 4);
                _uow.UserRepository.Update(user);

                await _uow.CommitAsync();

                return Result<UpdateLocationResponse>.Success(new UpdateLocationResponse(true));
            }
            catch(Exception ex)
            {
                return Result<UpdateLocationResponse>.Failure("Throw Exception " + ex.Message);
            }
        }

        private Result<UpdateLocationResponse> ValidateRequest(UpdateLocationRequest request)
        {
            if (string.IsNullOrEmpty(request.UserId))
                return Result<UpdateLocationResponse>.Failure("Invalid UserId");
            if (request.Longtitude < -180 || request.Longtitude > 180)
                return Result<UpdateLocationResponse>.Failure("Invalid Longtitude");
            if (request.Latitude < -90 || request.Latitude > 90)
                return Result<UpdateLocationResponse>.Failure("Invalid Latitude");
            return Result<UpdateLocationResponse>.Success(null);
        }
    }
}