namespace Model.DTO.Delivery;

public record AddDeliveryDto(string Name, string Description, string Category, LocationDto StartLocation, LocationDto EndLocation, HashSet<string> Recipients, bool IsFragile);