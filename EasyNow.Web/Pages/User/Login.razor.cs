using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AntDesign;
using Blazored.LocalStorage;
using EasyNow.ApiClient;
using EasyNow.Dto.User;
using Microsoft.AspNetCore.Components;

namespace EasyNow.Web.Pages.User
{
    public partial class Login
    {
        private readonly LoginModel _model = new LoginModel();

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public MessageService MessageService { get; set; }

        [Inject]
        public IEasyNowApi ApiClient { get; set; }

        [Inject]
        public ILocalStorageService LocalStorageService { get; set; }

        public async Task UserLogin()
        {
            if (string.IsNullOrEmpty(this._model.UserName) || string.IsNullOrEmpty(this._model.Password))
            {
                await MessageService.Error("用户名或密码为空");
                return;
            }

            var result = await ApiClient.Login(this._model);
            if (result.Code == 0)
            {
                await LocalStorageService.SetItemAsync("token", result.Data.Token);
                NavigationManager.NavigateTo("/");
            }
            else
            {
                await MessageService.Error(result.Msg??"登录失败");
            }
        }
    }
}
