namespace Model.DTO;

public record UserDto(string Username, string FirstName, string LastName, string Email, string Phone, bool IsDeliveryPerson);