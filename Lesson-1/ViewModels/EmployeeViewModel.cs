using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.ViewModels;

public class EmployeeViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Фамилия обязательна к заполнению")]
    [Display(Name = "Фамилия")]
    [StringLength(maximumLength:255,ErrorMessage = "Значение должно быть в диапазоне от 2-х до 255 символов",MinimumLength = 2)]
    [RegularExpression("^[A-Z][a-z]+|^[А-Я,Ё][а-я,ё]+", ErrorMessage = "Ошибка формата")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Имя обязательно к заполнению")]
    [StringLength(maximumLength: 255, ErrorMessage = "Значение должно быть в диапазоне от 2-х до 255 символов", MinimumLength = 2)]
    [RegularExpression("^[A-Z][a-z]+|^[А-Я,Ё][а-я,ё]+",ErrorMessage = "Ошибка формата")]
    [Display(Name = "Имя")]
    public string Name { get; set; }

    [Display(Name = "Отчество")]
    [StringLength(maximumLength: 255, ErrorMessage = "Значение должно быть в диапазоне от 2-х до 255 символов")]
    [RegularExpression("(^[A-Z][a-z]+|^[А-Я,Ё][а-я,ё]+)?", ErrorMessage = "Ошибка формата")]
    public string Patronymic { get; set; }

    [Range(18,55,ErrorMessage = "Введите число в диапазоне от 18 до 55")]
    public int Age { get; set; }

    [Range(15000, 200000, ErrorMessage = "Введите число в диапазоне от 15000 до 200000")]
    public decimal Income { get; set; }
}

