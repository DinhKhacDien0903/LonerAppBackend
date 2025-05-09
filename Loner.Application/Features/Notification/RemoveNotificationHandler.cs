using Loner.Application.DTOs;

namespace Loner.Application.Features.Notification;

public class RemoveNotificationHandler : IRequestHandler<RemoveNotificationRequest, Result<UpdateNotificationResponse>>
{
    private readonly IUnitOfWork _uow;
    public RemoveNotificationHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<UpdateNotificationResponse>> Handle(RemoveNotificationRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Notification.Id))
            return Result<UpdateNotificationResponse>.Failure("Unauthorized. Need Id paramater!");

        var IsSuccess = await _uow.NotificationRepository.UpdateIsDeleteNotificationAsync(request.Notification.Id, request.Notification.IsDeleted);
        await _uow.CommitAsync();
        if (!IsSuccess)
            return Result<UpdateNotificationResponse>.Failure("Failed to update notification!");
        return Result<UpdateNotificationResponse>.Success(new UpdateNotificationResponse
            ("Notification updated successfully!", true));
    }
}