namespace Loner.Application.DTOs;

public class ChatBot
{
    public record PromptRequest(SendMessageRequestDto Request) : IRequest<Result<PromptResponse>>;
    public record PromptResponse(string Response, bool IsSuccess);
    public class ContentRequest
    {
        public Content[] contents { get; set; }
    }
    public class Content
    {
        public string role { get; set; }
        public Part[] parts { get; set; }
    }
    public class Part
    {
        public string text { get; set; }
    }
}