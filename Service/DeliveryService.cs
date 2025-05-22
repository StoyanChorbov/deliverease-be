using Microsoft.IdentityModel.Tokens;
using Model;
using Model.DTO.Delivery;
using Model.DTO.Location;
using Repository;

namespace Service;

public class DeliveryService(
    DeliveryRepository deliveryRepository,
    LocationService locationService,
    UserService userService)
{
    public async Task<string> AddDeliveryAsync(DeliveryAddDto deliveryAddDto, string username)
    {
        var startLocation = await locationService.AddLocationAsync(deliveryAddDto.StartLocation);
        var endLocation = await locationService.AddLocationAsync(deliveryAddDto.EndLocation);
        var recipients = await userService.GetAllByUsernamesAsync(deliveryAddDto.Recipients);
        var sender = await userService.GetUserByUsernameAsync(username) ?? throw new Exception("User not found");

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
        return delivery.Id.ToString();
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

    public async Task<List<UserDeliveryDto>> GetPastDeliveriesAsync(string username)
    {
        var pastDeliveries = await deliveryRepository.GetPastDeliveriesAsync(username);
        return pastDeliveries
            .Select(ToUserDeliveryDto)
            .ToList();
    }

    public async Task<List<DeliveryListDto>> GetAllByStartingAndEndingLocation(string startingLocationRegion,
        string endingLocationRegion)
    {
        return (await deliveryRepository.GetAllByStartingAndEndingLocation(startingLocationRegion,
                endingLocationRegion))
            .Select(ToDeliveryListDto)
            .ToList();
    }

    public async Task<Tuple<List<DeliveryListDto>, List<DeliveryListDto>>> GetCurrentDeliveriesAsync(string username)
    {
        var toDeliver = await deliveryRepository.GetAllToDeliver(username);
        var toReceive = await deliveryRepository.GetAllToReceive(username);

        return new Tuple<List<DeliveryListDto>, List<DeliveryListDto>>(
            toDeliver.Select(ToDeliveryListDto).ToList(),
            toReceive.Select(ToDeliveryListDto).ToList()
        );
    }

    public async Task SetDeliveryDelivererAsync(string deliveryId, string username)
    {
        var user = await userService.GetUserByUsernameAsync(username);
        await deliveryRepository.SetDeliveryDelivererAsync(deliveryId, user);
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
        delivery.Sender = await userService.GetUserByUsernameAsync(deliveryDto.Sender);
        delivery.Recipients = await userService.GetAllByUsernamesAsync(deliveryDto.Recipients);
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
                delivery.StartingLocation.Place,
                delivery.StartingLocation.Street,
                delivery.StartingLocation.Number,
                delivery.StartingLocation.Region,
                delivery.StartingLocation.Latitude,
                delivery.StartingLocation.Longitude
            ),
            delivery.StartingLocationRegion,
            new LocationDto(
                delivery.EndingLocation.Place,
                delivery.EndingLocation.Street,
                delivery.EndingLocation.Number,
                delivery.EndingLocationRegion,
                delivery.EndingLocation.Latitude,
                delivery.EndingLocation.Longitude
            ),
            delivery.EndingLocationRegion,
            delivery.Sender.UserName ?? throw new Exception("User not found"),
            delivery.Deliverer?.UserName,
            delivery.Recipients.IsNullOrEmpty()
                ? []
                : delivery.Recipients.Select(r => r.UserName ?? "").ToList(),
            delivery.IsFragile
        );
    }

    private static FindableDeliveryDto ToFindableDto(Delivery delivery) =>
        new(
            delivery.Id.ToString(),
            delivery.Name,
            delivery.Category.ToString(),
            new LocationDto(
                delivery.StartingLocation.Place,
                delivery.StartingLocation.Street,
                delivery.StartingLocation.Number,
                delivery.StartingLocation.Region,
                delivery.StartingLocation.Latitude,
                delivery.StartingLocation.Longitude
            ),
            new LocationDto(
                delivery.EndingLocation.Place,
                delivery.EndingLocation.Street,
                delivery.EndingLocation.Number,
                delivery.EndingLocation.Region,
                delivery.EndingLocation.Latitude,
                delivery.EndingLocation.Longitude
            ),
            delivery.IsFragile
        );

    private static UserDeliveryDto ToUserDeliveryDto(Delivery delivery) =>
        new(
            delivery.Id.ToString(),
            delivery.Name,
            delivery.StartingLocationRegion,
            delivery.EndingLocationRegion,
            delivery.Category.ToString(),
            delivery.IsFragile
        );

    private static DeliveryListDto ToDeliveryListDto(Delivery delivery) =>
        new(
            delivery.Id.ToString(),
            delivery.Name,
            new LocationDto(
                delivery.StartingLocation.Place,
                delivery.StartingLocation.Street,
                delivery.StartingLocation.Number,
                delivery.StartingLocation.Region,
                delivery.StartingLocation.Latitude,
                delivery.StartingLocation.Longitude
            ),
            new LocationDto(
                delivery.EndingLocation.Place,
                delivery.EndingLocation.Street,
                delivery.EndingLocation.Number,
                delivery.EndingLocation.Region,
                delivery.EndingLocation.Latitude,
                delivery.EndingLocation.Longitude
            ),
            delivery.Category.ToString(),
            delivery.IsFragile
        );
}