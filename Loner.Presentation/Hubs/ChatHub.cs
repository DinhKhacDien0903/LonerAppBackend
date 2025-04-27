using Loner.Application.DTOs;
using Loner.Application.Interfaces;
using Loner.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Loner.Presentation.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<UserEntity> _userManager;

        private readonly IChatHubService _chatHubService;

        private readonly IHubContext<NotificationHub> _notificationHubContext;

        private const int MAX_MESSAGE_LENGTH = 500;

        private const string MESSAGE_NOTIFICATION = "You have a new message";
        public ChatHub(
            UserManager<UserEntity> userManager,
            IChatHubService chatHubService,
            IHubContext<NotificationHub> notificationHubContext
            )
        {
            _userManager = userManager;
            _chatHubService = chatHubService;
            _notificationHubContext = notificationHubContext;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var user = await ValidateCurrentAccount();

                await Clients.Others.SendAsync("UserConnected", user.Id);


                await base.OnConnectedAsync();
            }
            catch
            (Exception e)
            {
                await Clients.Caller.SendAsync("UserNotConnected", e.Message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = await ValidateCurrentAccount();

            await Clients.Others.SendAsync("UserDisConnected", user.Id);

            await base.OnDisconnectedAsync(exception);
        }


        #region'Chat Person'
        public async Task<SendMessageRequestDto> SendMessageToPerson(SendMessageRequestDto param)
        {
            var sender = await ValidateCurrentAccount();

            var reciver = await _userManager.FindByIdAsync(param.ReceiverId ?? "");

            param.Content = param.Content.Trim();

            if (!await ValidateMessage(param, reciver?.Id ?? ""))
            {
                return new SendMessageRequestDto();
            }

            var message = await _chatHubService.AddMessagePersonAsync(param);

            await NotifyReceiverAsync(param?.ReceiverId ?? "", message);

            try
            {
                var isNotificationExist = await _chatHubService.IsNotificationExist(sender.Id, param?.ReceiverId ?? "", param?.ReceiverId ?? "");

                if (!isNotificationExist)
                {
                    var notification = await SaveNotificationToUser(param?.SenderId ?? "", param?.ReceiverId ?? "", MESSAGE_NOTIFICATION);

                    await _notificationHubContext.Clients.User(param?.ReceiverId ?? "").SendAsync("ReceiveNotification", notification);
                }
            }
            catch (Exception c)
            {

                var x = c.Message;
            }
            return message;
        }

        //public async Task ReadMessageNotification(NotificationRequest param)
        //{

        //    var sender = await ValidateCurrentAccount();

        //    try
        //    {
        //        var notification = await _chatHubService.ReadMessageNotificationAsync(sender.Id, param?.RecieverId, param?.GroupId);

        //        await _notificationHubContext.Clients.User(sender.Id).SendAsync("ReadMessageNotification", notification);
        //    }
        //    catch (Exception c)
        //    {

        //        var x = c.Message;
        //    }
        //}

        //public async Task OnUserTyping(string reciverId)
        //{
        //    await Clients.User(reciverId).SendAsync("ReciverTypingNotification", true);
        //}

        //public async Task StoppedUserTyping(string reciverId)
        //{
        //    await Clients.User(reciverId).SendAsync("ReciverTypingNotification", false);
        //}

        //private async Task UpdateStatusActiveUser(string userId, bool isActive)
        //{
        //    await _chatHubService.UpdateStatusActiveUser(userId, isActive);
        //}

        //public async Task RemoveMessage(string messageId, string reciverId)
        //{
        //    if (!string.IsNullOrEmpty(messageId))
        //    {
        //        await _chatHubService.RemoveMessage(messageId);

        //        var response = new MessagePersonResponse
        //        {
        //            MessageID = messageId,
        //            IsDelete = true
        //        };

        //        await NotifyReceiverAsync(reciverId, response);
        //    }
        //}


        //public async Task<string> UpdateMessage(UpdateMessageRequest param)
        //{
        //    if (!string.IsNullOrEmpty(param.MessageId) && !string.IsNullOrEmpty(param.Content))
        //    {
        //        var updateDatetime = DateTime.UtcNow;

        //        await _chatHubService.UpdateMessage(param, updateDatetime);

        //        var response = new MessagePersonResponse
        //        {
        //            MessageID = param.MessageId,
        //            Content = param.Content,
        //            UpdateAt = updateDatetime,
        //            ReactionByUser = param.ReactionByUser
        //        };

        //        await NotifyReceiverAsync(param.ReciverId, response);

        //    }
        //    return param.MessageId;
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

        private async Task<bool> ValidateMessage(SendMessageRequestDto request, string receiver)
        {
            request.Content = request?.Content?.Trim();

            if (string.IsNullOrEmpty(request.Content))
            {
                await Clients.Caller.SendAsync("MessageValidateError", "Message cannot be empty");
                return false;
            }

            if (IsMessageTooLong(request?.Content ?? ""))
            {
                await Clients.Caller.SendAsync("MessageValidateTooLarge", $"Message is too long. Max is {MAX_MESSAGE_LENGTH} characters");
                return false;
            }

            return true;
        }

        private bool IsMessageTooLong(string content)
        {
            return content?.Length > MAX_MESSAGE_LENGTH;
        }

        //private async Task<MessageViewModel> SaveMessage(string senderId, SendMessageToPersonRequest request)
        //{
        //    var sendDatetime = DateTime.UtcNow;

        //    var messageViewModel = new MessageViewModel
        //    {
        //        SenderID = senderId,
        //        ReciverID = request.ReciverId,
        //        Content = request.Content,
        //        CreatedAt = sendDatetime,
        //        Images = request.Images,
        //        Symbol = request.Symbol
        //    };

        //    return await _chatHubService.AddMessagePersonAsync(messageViewModel);
        //}

        //private async Task SaveMessageImages(string messageId, List<string> images)
        //{
        //    var messageImages = images.Select(image => new MessageImageViewModel
        //    {
        //        MessageImageID = Guid.NewGuid().ToString(),
        //        MessageID = messageId,
        //        ImageUrl = image
        //    }).ToList();

        //    await _chatHubService.AddMessageImagesAsync(messageImages);
        //}

        //private MessagePersonResponse CreateMessageResponse(MessageViewModel message, SendMessageToPersonRequest request)
        //{
        //    return new MessagePersonResponse
        //    {
        //        SenderID = message.SenderID,
        //        MessageID = message.MessageID,
        //        Content = request.Content,
        //        Images = request.Images,
        //        CreatedAt = message.CreatedAt,
        //        Symbol = request.Symbol
        //    };
        //}

        private async Task NotifyReceiverAsync(string receiverId, SendMessageRequestDto response)
        {
            await Clients.User(receiverId).SendAsync("ReceiveSpecificMessage", response);
        }

        private async Task<NotificationDto> SaveNotificationToUser(string senderId, string reciverId, string message)
        {
            var sendDatetime = DateTime.UtcNow;

            var notificationViewModel = new NotificationDto
            {
                SenderId = senderId,
                ReceiverId = reciverId,
                Messeage = message,
                CreatedAt = sendDatetime,
                UpdatedAt = sendDatetime,
                Type = 2
            };

            return await _chatHubService.AddNotificationToUserAsync(notificationViewModel);
        }
        #endregion
    }
}