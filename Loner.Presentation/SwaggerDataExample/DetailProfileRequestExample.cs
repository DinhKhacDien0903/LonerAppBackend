using Swashbuckle.AspNetCore.Filters;
using static Loner.Application.DTOs.ProfileDetail;
namespace Loner.Presentation.SwaggerDataExample;

public class DetailProfileRequestExample : IExamplesProvider<GetProfileDetailRequest>
{
    public GetProfileDetailRequest GetExamples()
    {
        return new GetProfileDetailRequest(
            UserId: Constants.DefaultIdOfUser1
        );
    }
}