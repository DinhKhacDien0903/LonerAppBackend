namespace Loner.Application.DTOs
{
    public class Location
    {
        public record UpdateLocationRequest(string UserId, double Longtitude, double Latitude) : IRequest<Result<UpdateLocationResponse>>;
        public record UpdateLocationResponse(bool IsSucess);
        public record GetMemberByLocationAndRadiusRequest(string UserId, double Longtitude, double Latitude, double Radius) : IRequest<Result<GetMemberByLocationAndRadiusResponse>>;
        public record GetMemberByLocationAndRadiusResponse(List<UserLocationDto> Users);
    }
}