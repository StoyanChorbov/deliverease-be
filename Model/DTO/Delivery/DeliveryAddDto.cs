namespace Model.DTO.Delivery;

public record DeliveryAddDto(
    string Name,
    string Description,
    string Category,
    LocationDto StartLocation,
    string StartLocationRegion,
    LocationDto EndLocation,
    string EndLocationRegion,
    List<string> Recipients,
    bool IsFragile);