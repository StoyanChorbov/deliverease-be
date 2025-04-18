using Model;
using Model.DTO.Delivery;
using Repository;

namespace Service;

public class LocationService(LocationRepository locationRepository)
{
    public async Task<Location> AddLocationAsync(LocationDto locationDto)
    {
        var location = new Location
        {
            Address = locationDto.Address,
            City = locationDto.City,
            Latitude = locationDto.Latitude,
            Longitude = locationDto.Longitude
        };
        await locationRepository.AddAsync(location);
        return location;
    }
}