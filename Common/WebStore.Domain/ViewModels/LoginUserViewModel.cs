using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Domain.ViewModels;

public class LoginUserViewModel
{
    [Required]
    [Display(Name="User Name")]
    public string UserName { get; set; }

    [Required]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember Me")] public bool RememberMe { get; set; } = false;

    [HiddenInput(DisplayValue = false)]
    public string? ReturnUrl { get; set; }
}