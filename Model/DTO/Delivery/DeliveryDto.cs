namespace Model.DTO.Delivery;

public record DeliveryDto(
    string Id,
    string Name,
    string Description,
    string Category,
    LocationDto StartingLocation,
    int StartingLocationRegion,
    LocationDto EndingLocation,
    int EndingLocationRegion,
    string Sender,
    string? Deliverer,
    List<string> Recipients,
    bool IsFragile
);