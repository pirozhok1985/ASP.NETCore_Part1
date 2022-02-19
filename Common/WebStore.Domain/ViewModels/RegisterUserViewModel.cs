using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Domain.ViewModels;

public class RegisterUserViewModel
{
    [Required]
    [Display(Name = "Имя пользователя")]
    [Remote("ValidateName","Account")]
    public string UserName { get; set; }

    [Required]
    [Display(Name = "Пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Display(Name = "Подтверждение пароля")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
}