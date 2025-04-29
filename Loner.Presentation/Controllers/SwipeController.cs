using Loner.Application.DTOs;
using Loner.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using static Loner.Application.DTOs.Matches;
using static Loner.Application.DTOs.Profile;
using static Loner.Application.DTOs.Swipe;

namespace Loner.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //TODO: Add authorization.
    // [Authorize]
    public class SwipeController : BaseController
    {
        private const string MESSAGE_NOTIFICATION_TITLE = "New match";
        private readonly IMediator _mediator;
        private readonly IHubContext<Hubs.NotificationHub> _notificationHubContext;
        private readonly ISwipeService _swipeService;
        public SwipeController(
            IMediator mediator,
            IHubContext<Hubs.NotificationHub> notificationHubContext,
            ISwipeService swipeService)
        {
            _mediator = mediator;
            _notificationHubContext = notificationHubContext;
            _swipeService = swipeService;
        }

        [HttpPost("swipe-user")]
        public async Task<IActionResult> Swipe([FromBody] SwipeRequest request)
        {
            var result = await _mediator.Send(request);
            if (result.IsSuccess && result.Data != null && result.Data.IsMatch)
            {
                var notification = await SaveNotificationToUser(
                    request?.SwiperId ?? "", request?.SwipedId ?? "", "Có người đã thích bạn" , MESSAGE_NOTIFICATION_TITLE);

                await _notificationHubContext.Clients.User(request?.SwipedId ?? "").SendAsync("ReceiveNotification", notification);
            }

            return HandleResult(result);
        }

        [HttpGet("profiles")]
        public async Task<IActionResult> GetProfiles([FromQuery] GetProfilesRequest request)
        {
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpGet("matches")]
        public async Task<IActionResult> GetMatches([FromQuery] GetMatchesRequest request)
        {
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        private async Task<NotificationDto> SaveNotificationToUser(
            string senderId, string reciverId, string message, string title, string? imageUrl = "", string matchId = "")
        {
            var sendDatetime = DateTime.UtcNow;

            var notificationViewModel = new NotificationDto
            {
                SenderId = senderId,
                ReceiverId = reciverId,
                Messeage = message,
                CreatedAt = sendDatetime,
                UpdatedAt = sendDatetime,
                Title = title,
                NotificationImage = imageUrl,
                Subtitle = MESSAGE_NOTIFICATION_TITLE,
                RelatedId = matchId,
                Type = 1
            };

            return await _swipeService.AddNotificationToUserAsync(notificationViewModel);
        }
    }
}