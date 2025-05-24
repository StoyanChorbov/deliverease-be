namespace Model.DTO.Delivery;

public record UserDeliveryDto(
    string Id,
    string Name,
    string StartLocationRegion,
    string EndLocationRegion,
    string Category,
    bool IsFragile
);