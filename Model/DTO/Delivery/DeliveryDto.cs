namespace Model.DTO.Delivery;

public record DeliveryDto(
    string Name,
    string Description,
    string Category,
    LocationDto StartingLocation,
    string StartingLocationRegion,
    LocationDto EndingLocation,
    string EndingLocationRegion,
    string Sender,
    string? Deliverer,
    List<string> Recipients,
    bool IsFragile
);