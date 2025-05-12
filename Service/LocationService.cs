using Model;
using Model.DTO.Delivery;
using Model.DTO.Location;
using Repository;

namespace Service;

public class LocationService(LocationRepository locationRepository)
{
    public async Task<Location> AddLocationAsync(LocationDto locationDto)
    {
        var location = new Location
        {
            Place = locationDto.Place,
            Street = locationDto.Street,
            Number = locationDto.Number,
            Region = locationDto.Region,
            Latitude = locationDto.Latitude,
            Longitude = locationDto.Longitude
        };
        await locationRepository.AddAsync(location);
        return location;
    }
}