using System.ComponentModel.DataAnnotations;
using Model.DTO.Location;

namespace Model.DTO.Delivery;

public record DeliveryAddDto(
    [Required] string Name,
    [Required] string Description,
    [Required] string Category,
    [Required] LocationDto StartLocation,
    [Required] string StartLocationRegion,
    [Required] LocationDto EndLocation,
    [Required] string EndLocationRegion,
    [Required] List<string> Recipients,
    [Required] bool IsFragile
);