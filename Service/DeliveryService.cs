using Model;
using Model.DTO.Delivery;
using Repository;

namespace Service;

public class DeliveryService(
    DeliveryRepository deliveryRepository,
    LocationService locationService,
    UserService userService)
{
    public async Task AddDeliveryAsync(DeliveryAddDto deliveryAddDto, string userToken)
    {
        var startLocation = await locationService.AddLocationAsync(deliveryAddDto.StartLocation);
        var endLocation = await locationService.AddLocationAsync(deliveryAddDto.EndLocation);
        var recipients = await userService.GetAllByUsernames(deliveryAddDto.Recipients);
        var sender = await userService.GetByJwtToken(userToken) ?? throw new Exception("User not found");

        var delivery = new Delivery
        {
            Name = deliveryAddDto.Name,
            Description = deliveryAddDto.Description,
            Category = Enum.Parse<DeliveryCategory>(deliveryAddDto.Category),
            StartingLocation = startLocation,
            StartingLocationRegion = deliveryAddDto.StartLocationRegion,
            EndingLocation = endLocation,
            EndingLocationRegion = deliveryAddDto.EndLocationRegion,
            Sender = sender,
            Recipients = recipients,
            IsFragile = deliveryAddDto.IsFragile
        };

        await deliveryRepository.AddAsync(delivery);
    }

    public async Task<DeliveryDto> GetDeliveryAsync(Guid id)
    {
        return ToDto(await deliveryRepository.GetAsync(id));
    }

    public async Task<List<DeliveryDto>> GetAllDeliveriesAsync()
    {
        return (await deliveryRepository.GetAllAsync()).Select(ToDto).ToList();
    }

    public async Task<List<DeliveryDto>> GetAllByStartingAndEndingLocation(string startingLocationRegion,
        string endingLocationRegion)
    {
        return (await deliveryRepository.GetAllByStartingAndEndingLocation(startingLocationRegion, endingLocationRegion))
            .Select(ToDto)
            .ToList();
    }

    public async Task UpdateDeliveryAsync(Guid id, DeliveryDto deliveryDto)
    {
        // TODO: Update with custom starting and ending regions
        var delivery = await deliveryRepository.GetAsync(id);
        delivery.Name = deliveryDto.Name;
        delivery.Description = deliveryDto.Description;
        delivery.Category = Enum.Parse<DeliveryCategory>(deliveryDto.Category);
        delivery.StartingLocation = await locationService.AddLocationAsync(deliveryDto.StartingLocation);
        delivery.StartingLocationRegion = deliveryDto.StartingLocation.City;
        delivery.EndingLocation = await locationService.AddLocationAsync(deliveryDto.EndingLocation);
        delivery.EndingLocationRegion = deliveryDto.EndingLocation.City;
        delivery.Sender = await userService.GetUserByUsername(deliveryDto.Sender);
        delivery.Recipients = await userService.GetAllByUsernames(deliveryDto.Recipients);
        await deliveryRepository.UpdateAsync(delivery);
    }

    public async Task DeleteDeliveryAsync(Guid id)
    {
        await deliveryRepository.DeleteAsync(id);
    }

    private static DeliveryDto ToDto(Delivery delivery)
    {
        return new DeliveryDto(
            delivery.Name,
            delivery.Description,
            delivery.Category.ToString(),
            new LocationDto(
                delivery.StartingLocation.Address,
                delivery.StartingLocation.City,
                delivery.StartingLocation.Latitude,
                delivery.StartingLocation.Longitude
            ),
            delivery.StartingLocationRegion,
            new LocationDto(
                delivery.EndingLocation.Address,
                delivery.EndingLocation.City,
                delivery.EndingLocation.Latitude,
                delivery.EndingLocation.Longitude
            ),
            delivery.EndingLocationRegion,
            delivery.Sender.UserName ?? throw new Exception("User not found"),
            delivery.Deliverer?.UserName ?? throw new Exception("User not found"),
            delivery.Recipients.Select(r => r.UserName ?? "").ToList(),
            delivery.IsFragile
        );
    }
}