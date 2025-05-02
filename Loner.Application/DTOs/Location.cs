namespace Loner.Application.DTOs
{
    public class Location
    {
        public record UpdateLocationRequest(string UserId, string Longitude, string Latitude) : IRequest<Result<UpdateLocationResponse>>;
        public record UpdateLocationResponse(bool IsSuccess);
        //public record GetMemberByLocationAndRadiusRequest(string UserId, string Longitude, string Latitude, double Radius) : IRequest<Result<GetMemberByLocationAndRadiusResponse>>;
        public class GetMemberByLocationAndRadiusRequest : IRequest<Result<GetMemberByLocationAndRadiusResponse>>
        {
            public string UserId { get; init; }
            public string Longitude { get; init; }
            public string Latitude { get; init; }
            public double Radius { get; init; }

            [System.Text.Json.Serialization.JsonConstructor]
            public GetMemberByLocationAndRadiusRequest(string userId, string longitude, string latitude, double radius)
            {
                UserId = userId;
                Longitude = longitude;
                Latitude = latitude;
                Radius = radius;
            }
        }
        public record GetMemberByLocationAndRadiusResponse(List<UserLocationDto> Users);
    }
}