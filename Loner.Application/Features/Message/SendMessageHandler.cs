using Loner.Application.DTOs;
using Loner.Domain.Entities;
using static Loner.Application.DTOs.Message;

namespace Loner.Application.Features.Message
{
    public class SendMessageHandler : IRequestHandler<SendMessageRequest, Result<SendMessageResponse>>
    {
        private readonly IUnitOfWork _uow;
        public SendMessageHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<SendMessageResponse>> Handle(SendMessageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validateResult = ValidateMessage(request.MessageRequest);
                if (!validateResult.IsSuccess)
                    return validateResult;

                var newMessage = InitMessage(request.MessageRequest);
                await _uow.MessageRepository.AddAsync(newMessage);
                await _uow.CommitAsync();

                return Result<SendMessageResponse>.Success(new SendMessageResponse(newMessage.Id));
            }
            catch (Exception ex)
            {
                return Result<SendMessageResponse>.Failure(ex.Message);
            }
        }

        private Result<SendMessageResponse> ValidateMessage(SendMessageRequestDto messageRequest)
        {
            if (string.IsNullOrEmpty(messageRequest.SenderId))
                return Result<SendMessageResponse>.Failure("Invalid SenderId");
            if (string.IsNullOrEmpty(messageRequest.Content))
                return Result<SendMessageResponse>.Failure("Invalid Content");
            if (string.IsNullOrEmpty(messageRequest.MatchId))
                return Result<SendMessageResponse>.Failure("Invalid MatchId");
            return Result<SendMessageResponse>.Success(null);
        }

        private MessageEntity InitMessage(SendMessageRequestDto messageRequest)
        {
            return new MessageEntity
            {
                Id  = Guid.NewGuid().ToString(),
                SenderId = messageRequest.SenderId,
                MatchId = messageRequest.MatchId,
                Content = messageRequest.Content,
                CreatedAt = messageRequest.SendTime,
                IsImage = messageRequest.IsImage,
                IsMessageOfChatBot = messageRequest.IsMessageOfChatBot
            };
        }
    }
}