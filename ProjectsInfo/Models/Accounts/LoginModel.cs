using System.ComponentModel.DataAnnotations;

namespace ProjectsInfo.Models.Accounts
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
        
        public string ReturnUrl { get; set; }
    }
}