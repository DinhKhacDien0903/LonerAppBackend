using Loner.Application.DTOs;

namespace Loner.Application.Features.Notification;

public class ClearNotificationsHandler : IRequestHandler<ClearNotificationRequest, Result<ClearNotificationResponse>>
{
 private readonly IUnitOfWork _uow;
    public ClearNotificationsHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<ClearNotificationResponse>> Handle(ClearNotificationRequest request, CancellationToken cancellationToken)
    {
         if (string.IsNullOrEmpty(request.UserId))
            return Result<ClearNotificationResponse>.Failure("Unauthorized. Need Id paramater!");

        var IsSuccess = await _uow.NotificationRepository.ClearNotificationsAsync(request.UserId);
        await _uow.CommitAsync();
        if (!IsSuccess)
            return Result<ClearNotificationResponse>.Failure("Failed to clear notification!");
        return Result<ClearNotificationResponse>.Success(new ClearNotificationResponse
            ("Notification clear successfully!", true));
    }
}