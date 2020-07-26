using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace EasyNow.Web.Pages.User
{
    public partial class Login
    {
        private readonly User _model = new User();

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public void NavLogin()
        {
            NavigationManager.NavigateTo("/");
        }
    }
    public class User
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string Captcha { get; set; }
    }
}
