using System.ComponentModel.DataAnnotations;

namespace Model.DTO.User;

public record UserRegisterDto(
    string Username,
    [MinLength(8)]
    string Password,
    string FirstName,
    string LastName,
    [EmailAddress]
    string Email,
    string PhoneNumber = "");