using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ChatServer.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Пожалуйста, введите ваш Nickname")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите пароль.")] 
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Пожалуйста, подтвердите пароль.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}