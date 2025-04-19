using Microsoft.AspNetCore.SignalR;
using Service;

namespace Application.Hubs;

public class LocationsHub(LocationService locationService) : Hub
{
    public async Task GetDelivererLocation(string delivererUsername)
    {
        // This method will be called from the client-side to get the location of a specific deliverer
        // var location = await GetLocationFromDatabase(delivererId); // Implement this method to fetch location from your database
        // await Clients.Caller.SendAsync("ReceiveDelivererLocation", location);
    }
}