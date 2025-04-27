using Loner.Application.DTOs;

namespace Loner.Application.Interfaces
{
    public interface IChatHubService
    {
        Task<SendMessageRequestDto> AddMessagePersonAsync(SendMessageRequestDto messageViewModel);

        Task<NotificationDto> AddNotificationToUserAsync(NotificationDto notifiationViewModel);

        Task<IEnumerable<SendMessageRequestDto>> GetAllNotificationMessageAsync(string userId);

        Task RemoveMessage(string messageId);

        Task<bool> IsNotificationExist(string senderId, string reciverId, string relatedId);
    }
}