using Loner.Application.DTOs;

namespace Loner.Application.Interfaces;

public interface ISwipeService
{
        Task<NotificationDto> AddNotificationToUserAsync(NotificationDto notifiationViewModel);
}
