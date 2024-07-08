using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace TechShop.Application.Services.UserServices.UserService
{
    public class UserHub(IUserService userService) : Hub
    {
        private static ConcurrentDictionary<string, string> OnlineUsers = new ConcurrentDictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            if (Context.User == null)
            {
                await Task.CompletedTask;
                return;
            }

            var email = Context.User.Claims.First(c => c.Type == ClaimTypes.Email).Value;
            var userId = await userService.GetUserId(email);

            if (userId != null)
                OnlineUsers.TryAdd(Context.ConnectionId, userId);

            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            OnlineUsers.TryRemove(Context.ConnectionId, out _);
            return base.OnDisconnectedAsync(exception);
        }

        public static bool IsUserOnline(string userId)
        {
            return OnlineUsers.Values.Contains(userId);
        }
    }
}
