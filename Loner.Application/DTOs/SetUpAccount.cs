namespace Loner.Application.DTOs;
//Request
public class SetUpNameRequest : IRequest<Result<SetUpResponse>>
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}

public class SetUpDOBRequest : IRequest<Result<SetUpResponse>>
{
    public string UserId { get; set; } = string.Empty;
    public DateTime Dob { get; set; } = DateTime.MinValue;
}

public class SetUpGenderRequest : IRequest<Result<SetUpResponse>>
{
    public string UserId { get; set; } = string.Empty;
    public bool Gender { get; set; } = false;
}

public class SetUpShowGenderRequest : IRequest<Result<SetUpResponse>>
{
    public string UserId { get; set; } = string.Empty;
    public bool ShowGender { get; set; } = false;
}

public class SetUpUniversityRequest : IRequest<Result<SetUpResponse>>
{
    public string UserId { get; set; } = string.Empty;
    public string University { get; set; } = string.Empty;
}

public class SetUpInterestRequest : IRequest<Result<SetUpResponse>>
{
    public string UserId { get; set; } = string.Empty;
    public IEnumerable<string> Interests { get; set; } = Enumerable.Empty<string>();
}

public class SetUpPhotosRequest : IRequest<Result<SetUpResponse>>
{
    public string UserId { get; set; } = string.Empty;
    public IEnumerable<string> Photos { get; set; } = Enumerable.Empty<string>();
}

//Response
public record SetUpResponse(bool IsSuccess, string? Message = null);