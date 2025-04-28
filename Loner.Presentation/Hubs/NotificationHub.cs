﻿using Loner.Application.DTOs;
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

        public async Task SendNotificationToUser(string friendId, string message)
        {
            var sender = await ValidateCurrentAccount();

            //var reciver = await _userManager.FindByIdAsync(friendId);

            var notifiation = await SaveNotificationToUser(sender.Id, friendId, message);

            await Clients.User(friendId).SendAsync("ReceiveNotification", notifiation);
        }

        //public async Task ReadMessageNotification(NotificationDto param)
        //{

        //    var sender = await ValidateCurrentAccount();

        //    try
        //    {
        //        var notification = await _chatHubService.ReadMessageNotificationAsync(sender.Id, param?.RecieverId, param?.GroupId);

        //        await Clients.User(sender.Id).SendAsync("ReadMessageNotificationEvent", notification);
        //    }
        //    catch (Exception c)
        //    {

        //        var x = c.Message;
        //    }
        //}

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

        private async Task<NotificationDto> SaveNotificationToUser(string senderId, string friendId, string message)
        {
            var sendDatetime = DateTime.UtcNow;

            var notificationViewModel = new NotificationDto
            {
                SenderId = senderId,
                ReceiverId = friendId,
                Messeage = message,
                CreatedAt = sendDatetime,
                UpdatedAt = sendDatetime,
                RelatedId = friendId,
                Type = 0
            };

            return await _chatHubService.AddNotificationToUserAsync(notificationViewModel);
        }
    }
}