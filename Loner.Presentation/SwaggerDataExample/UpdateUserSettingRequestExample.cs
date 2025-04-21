using System;
using Loner.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;
using static Loner.Application.DTOs.User;

namespace Loner.Presentation.SwaggerDataExample;

public class UpdateUserSettingRequestExample : IExamplesProvider<UpdateUserSettingRequest>
{
    public UpdateUserSettingRequest GetExamples()
    {
        return new UpdateUserSettingRequest
        {
            EditRequest = new EditSettingAccountRequest
            {
                UserId = "user123",
                PhoneNumber = "1234567890",
                Email = "123@gmail.com",
                Address = "123 Main St, City, Country",
                ShowGender = true,
                MinAge = 18,
                MaxAge = 30
            }
        };
    }
}