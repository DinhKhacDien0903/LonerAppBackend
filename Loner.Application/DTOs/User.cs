namespace Loner.Application.DTOs
{
    public class User
    {
        public record UpdateUserInforRequest(EditInforRequest EditRequest) : IRequest<Result<UpdateUserInforResponse>>;
        public record UpdateUserInforResponse(bool IsSuccess);
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
}