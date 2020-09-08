using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EasyNow.Bo.Abstractions;
using EasyNow.Dto.User;
using EasyNow.Utility.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EasyNow.Bo
{
    public class UserBo:BaseBo,IUserBo
    {
        public async Task<LoginResult> Login(LoginModel model)
        {
            var user = await this.Db.User.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Account == model.Account && e.Password == model.Password);
            if (user == null)
            {
                throw new MessageException("登录失败");
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,user.Account)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Scope.Resolve<IConfiguration>()["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken("easynow.me", "api", claims, expires: DateTime.Now.AddHours(1), signingCredentials: credentials);

            return new LoginResult
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken)
            };

        }
    }
}