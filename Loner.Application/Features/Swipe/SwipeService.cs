using Loner.Application.DTOs;
using Loner.Application.Interfaces;
using Loner.Domain.Entities;

namespace Loner.Application.Features.Swipe;

public class SwipeService : ISwipeService
{
    private readonly IUnitOfWork _uow;
    public SwipeService(IUnitOfWork uow)
    {
        _uow = uow;
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
}