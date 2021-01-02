using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ProjectsInfo.Models.Accounts
{
    public class RegistrationModel
    {
        [Required]
        [Display(Name="Имя пользователя")]
        public string Username { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Пароль")]
        public string Password { get; set; }
        
        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name="Подтвердите пароль")]
        public string PasswordConfirm { get; set; }
        
        [Display(Name = "Зарегистрироваться как менеджер")]
        public bool IsManager { get; set; }
    }
}