using System.ComponentModel.DataAnnotations;

namespace Model.DTO.User;

public record UserRegisterDto(
    [Required(ErrorMessage = "Username is required.")]
    [MinLength(4, ErrorMessage = "Username must be at least 4 characters long.")]
    string Username,
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    string Password,
    [Required(ErrorMessage = "First name is required.")]
    string FirstName,
    [Required(ErrorMessage = "Last name is required.")]
    string LastName,
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    string Email,
    string PhoneNumber = "");