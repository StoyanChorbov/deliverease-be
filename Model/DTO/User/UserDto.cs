namespace Model.DTO.User;

public record UserDto(
    string Username,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    bool IsDeliveryPerson);