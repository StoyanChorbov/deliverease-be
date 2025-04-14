using System.ComponentModel.DataAnnotations;

namespace Model.DTO.User;

public record UserLoginDto(
    [Required(ErrorMessage = "Username is required.")]
    string Username,
    [Required(ErrorMessage = "Password is required.")]
    string Password);