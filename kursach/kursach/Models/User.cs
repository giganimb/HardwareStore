using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace kursach.Models
{
    public class User
    {
        [Key]
        [Required(ErrorMessage = "Отсутствует логин")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Отсутствует пароль")]
        public string Password { get; set; }
        public string IsAdmin { get; set; }
        public List<SoldProduct> SoldProducts { get; set; }

        public User() { }

        public User(string login, string password, string isAdmin)
        {
            this.Login = login;
            this.Password = password;
            this.IsAdmin = isAdmin;
        }

    }
}
