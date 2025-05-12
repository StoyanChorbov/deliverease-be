namespace Model.DTO.Delivery;

public record UserDeliveryDto(
    string Id,
    string Name,
    string StartLocationRegion,
    string EndLocationRegion,
    string DeliveryCategory,
    bool IsFragile
);