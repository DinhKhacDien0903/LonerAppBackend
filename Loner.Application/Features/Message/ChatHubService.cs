using Loner.Application.DTOs;
using Loner.Domain.Entities;

namespace Loner.Application.Interfaces
{
    public class ChatHubService : IChatHubService
    {
        private readonly IUnitOfWork _uow;
        public ChatHubService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<SendMessageRequestDto> AddMessagePersonAsync(SendMessageRequestDto request)
        {
            try
            {
                var validateResult = ValidateMessage(request);
                var newMessage = InitMessage(request);
                if (validateResult)
                {
                    await _uow.MessageRepository.AddAsync(newMessage);
                    await _uow.CommitAsync();
                    request.MessageId = newMessage.Id;

                    return request;
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<NotificationDto> AddNotificationToUserAsync(NotificationDto request)
        {
            try
            {
                var entity = new NotificationEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = request.Title ?? "",
                    Subtitle = request.Subtitle ?? "",
                    NotificationImage = request.NotificationImage ?? "",
                    SenderId = request.SenderId,
                    ReceiverId = request.ReceiverId,
                    Content = request.Messeage ?? "",
                    RelatedId = request.RelatedId,
                    Type = request.Type,
                };

                var notification = await _uow.NotificationRepository.AddAsync(entity);
                await _uow.CommitAsync();

                request.Id = notification.Id;

                return request;

            }
            catch (Exception e)
            {
                var x = e.Message;
                return new NotificationDto();
            }
        }

        public Task<IEnumerable<SendMessageRequestDto>> GetAllNotificationMessageAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsNotificationExist(string senderId, string reciverId, string relatedId)
        {
            return await _uow.NotificationRepository.IsNotificationExist(senderId, reciverId, relatedId);
        }

        public Task RemoveMessage(string messageId)
        {
            throw new NotImplementedException();
        }

        private bool ValidateMessage(SendMessageRequestDto messageRequest)
        {
            if (string.IsNullOrEmpty(messageRequest.SenderId))
                return false;
            if (string.IsNullOrEmpty(messageRequest.Content))
                return false;
            if (string.IsNullOrEmpty(messageRequest.MatchId))
                return false;
            return true;
        }

        private MessageEntity InitMessage(SendMessageRequestDto messageRequest)
        {
            return new MessageEntity
            {
                Id = Guid.NewGuid().ToString(),
                SenderId = messageRequest.SenderId,
                MatchId = messageRequest.MatchId,
                Content = messageRequest.Content,
                CreatedAt = messageRequest.SendTime,
                IsImage = messageRequest.IsImage,
            };
        }
    }
}