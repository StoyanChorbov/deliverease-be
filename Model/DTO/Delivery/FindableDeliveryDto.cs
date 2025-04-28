namespace Model.DTO.Delivery;

public record FindableDeliveryDto(
    string Id,
    string Name,
    string Category,
    LocationDto StartingLocation,
    LocationDto EndingLocation,
    bool IsFragile
);