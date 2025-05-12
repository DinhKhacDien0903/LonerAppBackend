using System.Globalization;
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
                var longitude = double.Parse(request.Longitude, CultureInfo.InvariantCulture);
                var latitude = double.Parse(request.Latitude, CultureInfo.InvariantCulture);
                var validateResult = ValidateRequest(request);
                if (!validateResult.IsSuccess)
                    return validateResult;

                var user = await _uow.UserRepository.GetByIdAsync(request.UserId);
                if (user == null)
                    return Result<UpdateLocationResponse>.Failure("User not found");

                user.Longitude = Math.Round(longitude, 4);
                user.Latitude = Math.Round(latitude, 4);
                _uow.UserRepository.Update(user);

                await _uow.CommitAsync();

                return Result<UpdateLocationResponse>.Success(new UpdateLocationResponse(true));
            }
            catch (Exception ex)
            {
                return Result<UpdateLocationResponse>.Failure("Throw Exception " + ex.Message);
            }
        }

        private Result<UpdateLocationResponse> ValidateRequest(UpdateLocationRequest request)
        {
            var longitude = double.Parse(request.Longitude, CultureInfo.InvariantCulture);
            var latitude = double.Parse(request.Latitude, CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(request.UserId))
                return Result<UpdateLocationResponse>.Failure("Invalid UserId");
            if (longitude < -180 || longitude > 180)
                return Result<UpdateLocationResponse>.Failure("Invalid Longitude");
            if (latitude < -90 || latitude > 90)
                return Result<UpdateLocationResponse>.Failure("Invalid Latitude");
            return Result<UpdateLocationResponse>.Success(null);
        }
    }
}