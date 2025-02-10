using System.ComponentModel.DataAnnotations;

namespace Model.DTO;

public record UserRegisterDto(
    string Username,
    [MinLength(8)]
    string Password,
    string FirstName,
    string LastName,
    [EmailAddress]
    string Email,
    string Phone = "");