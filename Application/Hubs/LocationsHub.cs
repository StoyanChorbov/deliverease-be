using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Application.Hubs;

// Hub for real-time location sharing
public class LocationsHub : Hub
{
    // All current connections
    private static readonly ConcurrentDictionary<string, string> Connections = new();
    
    // Connect to the hub
    public override async Task OnConnectedAsync()
    {
        var username = Context.User?.Identity?.Name;
        if (username != null)
        {
            Connections[Context.ConnectionId] = username;
        }
        await base.OnConnectedAsync();
    }
    
    // Disconnect from the hub
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var username = Context.User?.Identity?.Name;
        if (username != null)
        {
            Connections.TryRemove(Context.ConnectionId, out _);
        }
        await base.OnDisconnectedAsync(exception);
    }
    
    // Get location from deliverer
    public async Task GetDelivererLocation(string delivererUsername)
    {
        if (Connections.TryGetValue(delivererUsername, out var connectionId))
        {
            await Clients.Client(connectionId).SendAsync("ReceiveLocationUpdate");
        }
    }

    // Send location to user
    public async Task RespondWithLocation(string requestedConnectionId, string latitude, string longitude)
    {
        await Clients.Client(requestedConnectionId).SendAsync("ReceiveLocationUpdate", Context.User?.Identity?.Name, latitude, longitude);
    }
}