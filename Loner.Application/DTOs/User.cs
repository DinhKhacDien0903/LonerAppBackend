namespace Loner.Application.DTOs
{
    public class User
    {
        public record UpdateUserInforRequest : IRequest<Result<UpdateUserInforResponse>>
        {
            public EditInforRequest EditRequest { get; init; } = new();
        }
        public record UpdateUserInforResponse(bool IsSuccess);

        // public record UpdateUserSettingRequest(EditSettingAccountRequest EditRequest) : IRequest<Result<UpdateUserSettingResponse>>;
        public record UpdateUserSettingRequest : IRequest<Result<UpdateUserSettingResponse>>
        {
            public EditSettingAccountRequest EditRequest { get; init; } = new();
        }
        public record UpdateUserSettingResponse(bool IsSuccess);
    }

    public class EditInforRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string? About { get; set; }
        public string? University { get; set; }
        public string? Work { get; set; }
        public bool Gender { get; set; }
        public IEnumerable<string> Photos { get; set; } = [];
        public IEnumerable<string> Interests { get; set; } = [];
    }

    public class EditSettingAccountRequest
    {
        public string UserId { get; set;} = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public bool ShowGender { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
    }
}