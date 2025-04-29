using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Application.Hubs;

public class LocationsHub : Hub
{
    private static readonly ConcurrentDictionary<string, string> Connections = new();
    
    public override async Task OnConnectedAsync()
    {
        var username = Context.User?.Identity?.Name;
        if (username != null)
        {
            Connections[Context.ConnectionId] = username;
        }
        await base.OnConnectedAsync();
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var username = Context.User?.Identity?.Name;
        if (username != null)
        {
            Connections.TryRemove(Context.ConnectionId, out _);
        }
        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task GetDelivererLocation(string delivererUsername)
    {
        if (Connections.TryGetValue(delivererUsername, out var connectionId))
        {
            await Clients.Client(connectionId).SendAsync("ReceiveLocationUpdate");
        }
    }

    public async Task RespondWithLocation(string requestedConnectionId, string latitude, string longitude)
    {
        await Clients.Client(requestedConnectionId).SendAsync("ReceiveLocationUpdate", Context.User?.Identity?.Name, latitude, longitude);
    }
}