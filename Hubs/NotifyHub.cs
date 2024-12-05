using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace GoWheels_WebAPI.Hubs
{
    public class NotifyHub : Hub
    {
        public static readonly ConcurrentDictionary<string, string> userConnectionsDic = new();

       

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                userConnectionsDic[userId] = Context.ConnectionId;
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = userConnectionsDic.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (!string.IsNullOrEmpty(userId))
            {
                userConnectionsDic.TryRemove(userId, out _);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
