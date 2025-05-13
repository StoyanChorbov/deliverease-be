using Model.DTO.Location;

namespace Model.DTO.Delivery;

public record DeliveryListDto(string Id, string Name, LocationDto StartingLocationDto,
    LocationDto EndingLocationDto, string Category, bool IsFragile);