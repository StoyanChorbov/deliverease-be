using Microsoft.IdentityModel.Tokens;
using Model;
using Model.DTO.Delivery;
using Repository;

namespace Service;

public class DeliveryService(
    DeliveryRepository deliveryRepository,
    LocationService locationService,
    UserService userService)
{
    public async Task AddDeliveryAsync(DeliveryAddDto deliveryAddDto, string username)
    {
        var startLocation = await locationService.AddLocationAsync(deliveryAddDto.StartLocation);
        var endLocation = await locationService.AddLocationAsync(deliveryAddDto.EndLocation);
        var recipients = await userService.GetAllByUsernames(deliveryAddDto.Recipients);
        var sender = await userService.GetUserByUsername(username) ?? throw new Exception("User not found");

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

        await userService.AddDeliveryRecipients(delivery, recipients.Select(r => r.UserName!).ToList());
        await deliveryRepository.AddAsync(delivery);
    }

    public async Task<DeliveryDto> GetDeliveryAsync(Guid id)
    {
        return ToDto(await deliveryRepository.GetAsync(id));
    }

    public async Task<List<FindableDeliveryDto>> GetAllDeliveriesAsync()
    {
        var deliveries = await deliveryRepository.GetAllAsync();
        return deliveries.Select(ToFindableDto).ToList();
    }

    public async Task<List<DeliveryDto>> GetAllByStartingAndEndingLocation(int startingLocationRegion,
        int endingLocationRegion)
    {
        return (await deliveryRepository.GetAllByStartingAndEndingLocation(startingLocationRegion, endingLocationRegion))
            .Select(ToDto)
            .ToList();
    }

    public async Task UpdateDeliveryAsync(Guid id, DeliveryDto deliveryDto)
    {
        var delivery = await deliveryRepository.GetAsync(id);
        delivery.Name = deliveryDto.Name;
        delivery.Description = deliveryDto.Description;
        delivery.Category = Enum.Parse<DeliveryCategory>(deliveryDto.Category);
        delivery.StartingLocation = await locationService.AddLocationAsync(deliveryDto.StartingLocation);
        delivery.StartingLocationRegion = deliveryDto.StartingLocationRegion;
        delivery.EndingLocation = await locationService.AddLocationAsync(deliveryDto.EndingLocation);
        delivery.EndingLocationRegion = deliveryDto.EndingLocationRegion;
        delivery.Sender = await userService.GetUserByUsername(deliveryDto.Sender);
        delivery.Recipients = await userService.GetAllByUsernames(deliveryDto.Recipients);
        delivery.IsFragile = deliveryDto.IsFragile;
        await deliveryRepository.UpdateAsync(delivery);
    }

    public async Task DeleteDeliveryAsync(Guid id)
    {
        await deliveryRepository.DeleteAsync(id);
    }

    private static DeliveryDto ToDto(Delivery delivery)
    {
        return new DeliveryDto(
            delivery.Id.ToString(),
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
            delivery.Deliverer?.UserName,
            delivery.Recipients.IsNullOrEmpty() ? [] : delivery.Recipients.Select(r => r.UserName ?? "").ToList(),
            delivery.IsFragile
        );
    }
    
    private static FindableDeliveryDto ToFindableDto(Delivery delivery) =>
        new FindableDeliveryDto(
            delivery.Id.ToString(),
            delivery.Name,
            delivery.Category.ToString(),
            new LocationDto(
                delivery.StartingLocation.Address,
                delivery.StartingLocation.City,
                delivery.StartingLocation.Latitude,
                delivery.StartingLocation.Longitude
            ),
            new LocationDto(
                delivery.EndingLocation.Address,
                delivery.EndingLocation.City,
                delivery.EndingLocation.Latitude,
                delivery.EndingLocation.Longitude
            ),
            delivery.IsFragile
        );
}