using static Loner.Application.DTOs.ChatBot;
using Model = Loner.Application.DTOs.ChatBot;
using System.Text;
using Newtonsoft.Json;
using static Loner.Application.DTOs.ChatBotContentResponse;
using Loner.Domain.Entities;
using Loner.Application.DTOs;
using Newtonsoft.Json.Serialization;

namespace Loner.Application.Features.ChatBot;

public class PromptHandler : IRequestHandler<PromptRequest, Result<PromptResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly HttpClient _httpClient;
    private string CurrentUserId = string.Empty;
    private const string DEFAULT_LANGUAGE_RESPONSE = "Hãy trả lời bằng tiếng việt";
    private const string DEFAULT_MAX_LINE = "Hãy trả lời ngắn gọn trong tối đa 10 dòng";
    private const string DEFAULT_FORMAT = "Hãy trả lời ngắt dòng, dấu chấm ngắt đầy đủ";
    private const string DEFAULT_FORMAT_BOLD = "Các từ khoá qua trọng phải đặt trong thẻ <b></b> không được đặt trong **";
    private const string DEFAULT_TONE = "Hãy trả lời với giọng điệu thân thiện, tự nhiên và hơi dí dỏm";
    private const string DEFAULT_VARIETY = "Sử dụng từ ngữ đa dạng, tránh lặp lại các cụm từ hoặc cấu trúc câu";
    private const string DEFAULT_VIETNAMESE_CONTEXT = "Ưu tiên sử dụng các cách diễn đạt, thành ngữ, tục ngữ (nếu phù hợp) và văn hóa giao tiếp đặc trưng của người Việt Nam";
    public PromptHandler(IUnitOfWork uow)
    {
        _uow = uow;
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };

        _httpClient = new HttpClient(handler);
    }

    public async Task<Result<PromptResponse>> Handle(PromptRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validateResult = ValidateMessage(request.Request);
            if (!validateResult.IsSuccess)
                return validateResult;
            CurrentUserId = request.Request.SenderId;
            var messages = await _uow.MessageRepository.GetAllMessageChatBotAsync(request.Request.MatchId);

            var result = await GenerateContentAsync(messages.ToList(), request.Request.Content);

            var botMessage = new MessageEntity
            {
                Id = Guid.NewGuid().ToString(),
                MatchId = request.Request.MatchId,
                SenderId = request.Request.ReceiverId ?? "", // TODO: Add id for chat bot in User table.
                Content = result,
                IsMessageOfChatBot = true,
                IsImage = false,
                CreatedAt = DateTime.UtcNow.AddMilliseconds(10)
            };

            await _uow.MessageRepository.AddAsync(InitMessage(request.Request));
            await _uow.MessageRepository.AddAsync(botMessage);
            await _uow.CommitAsync();

            if (!string.IsNullOrEmpty(result))
            {
                return Result<PromptResponse>.Success(new PromptResponse(result, true));
            }
            else
            {
                return Result<PromptResponse>.Failure("Error generating content.");
            }
        }
        catch (Exception ex)
        {
            return Result<PromptResponse>.Failure($"{ex.Message}");
        }
    }

    public async Task<string> GenerateContentAsync(List<MessageEntity> messages, string currentPrompt)
    {
        var contents = messages
            .Select(m => new Model.Content
            {
                role = m.SenderId == CurrentUserId ? "user" : "model",
                parts = new[] { new Model.Part { text = m.Content } }
            })
            .ToList();
        contents.Add(new Model.Content
        {
            role = "user",
            parts = new[] { new Model.Part { text = currentPrompt } }
        });
        contents.AddRange(ConfigRequest());

        var request = new ContentRequest { contents = contents.ToArray() };
        string jsonRequest = JsonConvert.SerializeObject(request, Formatting.Indented, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(Enviroments.GeminiURL, content);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var geminiResponse = JsonConvert.DeserializeObject<ContentResponse>(jsonResponse);
            return geminiResponse?.Candidates[0]?.Content?.Parts[0]?.Text ?? string.Empty;
        }
        else
        {
            throw new Exception("Chat bot is busy. Please try again later.");
        }
    }

    private List<Model.Content> ConfigRequest()
    {
        return new List<Model.Content>
        {
            new Model.Content
            {
                role = "user",
                parts = new[]
                {
                    new Model.Part { text = DEFAULT_FORMAT_BOLD },
                    new Model.Part { text = DEFAULT_LANGUAGE_RESPONSE },
                    new Model.Part { text = DEFAULT_MAX_LINE },
                    new Model.Part { text = DEFAULT_FORMAT },
                    new Model.Part { text = DEFAULT_TONE },
                    new Model.Part { text = DEFAULT_VARIETY},
                    new Model.Part { text = DEFAULT_VIETNAMESE_CONTEXT },
                }
            }
        };
    }

    private Result<PromptResponse> ValidateMessage(SendMessageRequestDto messageRequest)
    {
        if (string.IsNullOrEmpty(messageRequest.SenderId))
            return Result<PromptResponse>.Failure("Invalid SenderId");
        if (string.IsNullOrEmpty(messageRequest.Content))
            return Result<PromptResponse>.Failure("Invalid Content");
        if (string.IsNullOrEmpty(messageRequest.MatchId))
            return Result<PromptResponse>.Failure("Invalid MatchId");
        return Result<PromptResponse>.Success(null);
    }

    private MessageEntity InitMessage(SendMessageRequestDto messageRequest)
    {
        return new MessageEntity
        {
            Id = Guid.NewGuid().ToString(),
            SenderId = messageRequest.SenderId,
            MatchId = messageRequest.MatchId,
            Content = messageRequest.Content,
            IsImage = messageRequest.IsImage,
            IsMessageOfChatBot = messageRequest.IsMessageOfChatBot
        };
    }

}