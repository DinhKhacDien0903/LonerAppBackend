using Loner.Application.DTOs;
using Loner.Application.Interfaces;
using Loner.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Loner.Presentation.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly UserManager<UserEntity> _userManager;

        private readonly IChatHubService _chatHubService;
        private const string MESSAGE_NOTIFICATION_TITLE = "Cảnh báo!";
        private const string MESSAGE_NOTIFICATION = "Bạn đã nhận được một cảnh báo vi phạm";
        public NotificationHub(
            UserManager<UserEntity> userManager,
            IChatHubService chatHubService)
        {
            _userManager = userManager;
            _chatHubService = chatHubService;
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.Identity?.Name;

            if (userId != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = await ValidateCurrentAccount();

            await Clients.Others.SendAsync("UserDisConnected", user.Id);

            await base.OnDisconnectedAsync(exception);
        }

        //public async Task SendNotificationToUser(string friendId, string message)
        //{
        //    var sender = await ValidateCurrentAccount();

        //    //var reciver = await _userManager.FindByIdAsync(friendId);

        //    var notifiation = await SaveNotificationToUser(sender.Id, friendId, message);

        //    await Clients.User(friendId).SendAsync("ReceiveNotification", notifiation);
        //}

        public async Task<bool> SendWarningNotificationReportToUser(SendMessageRequestDto param)
        {
            var sender = await ValidateCurrentAccount();

            var reciver = await _userManager.FindByIdAsync(param.ReceiverId ?? "");

            param.Content = param.Content.Trim();

            try
            {
                var isNotificationExist = await _chatHubService.IsNotificationExist(sender.Id, param?.ReceiverId ?? "", param?.ReceiverId ?? "");

                if (!isNotificationExist)
                {
                    var notification = await SaveNotificationToUser(
                        sender?.Id ?? "", param?.ReceiverId ?? "", param?.Content ?? MESSAGE_NOTIFICATION,
                        "Cảnh báo từ quản trị viên", reciver?.AvatarUrl ?? "", param?.MatchId ?? "");

                    await Clients.User(param.ReceiverId ?? "").SendAsync("ReceiveNotification", notification);
                }

                return true;
            }
            catch (Exception c)
            {

                return false;
            }
        }

        private async Task<IdentityUser> ValidateCurrentAccount()
        {
            var x = Context.User;
            var user = await _userManager.GetUserAsync(Context.User);

            if (user == null)
            {
                await Clients.Caller.SendAsync("UserNotConnected", "You must login to chat!");

                Context.Abort();

                throw new Exception("UserNotConnected!");
            }

            return user;
        }

        private async Task<NotificationDto> SaveNotificationToUser(
           string senderId, string reciverId, string message, string title, string? imageUrl, string matchId)
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
                Type = 3
            };

            return await _chatHubService.AddNotificationToUserAsync(notificationViewModel);
        }
    }
}