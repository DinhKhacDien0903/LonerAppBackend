using Loner.Application.DTOs;

namespace Loner.Application.Features.Notification;

public class GetNotificationsHandler : IRequestHandler<GetNotificationsRequest, Result<GetNotificationsResponse>>
{
    private readonly IUnitOfWork _uow;
    public GetNotificationsHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    public async Task<Result<GetNotificationsResponse>> Handle(GetNotificationsRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.PaginationRequest.UserId))
            return Result<GetNotificationsResponse>.Failure("Unauthorized. Need UserId paramater!");

        var notifications = await _uow.NotificationRepository.GetByUserIdPaginatedAsync(request.PaginationRequest.UserId,
            request.PaginationRequest.ValidPageNumber, request.PaginationRequest.ValidPageSize);

        List<NotificationDto> results = new();
        foreach (var item in notifications)
        {
            var currentUser = await _uow.UserRepository.GetByIdAsync(item.SenderId);
            var data = new NotificationDto
            {
                SenderId = item.SenderId,
                ReceiverId = item.ReceiverId,
                Messeage = item.Content,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt,
                RelatedId = item.RelatedId,
                Type = item.Type,
                Title = item.Title,
                IsRead = item.IsRead,
                NotificationImage = currentUser?.AvatarUrl ?? ""
            };

            results.Add(data);
        }

        return Result<GetNotificationsResponse>.Success(new GetNotificationsResponse
            (
                new PaginatedResponse<NotificationDto>
                {
                    TotalItems = await _uow.NotificationRepository.GetTotalRecordByUserIdAsync(request.PaginationRequest.UserId ?? ""),
                    PageSize = request.PaginationRequest.ValidPageSize,
                    PageNumber = request.PaginationRequest.ValidPageNumber,
                    Items = results
                }
            ));
    }
}