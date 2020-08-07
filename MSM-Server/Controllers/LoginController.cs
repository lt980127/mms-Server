using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mms_Server.DAL;
using Mms_Server.Model;
using MSM_Server.Utility;
using Newtonsoft.Json;

namespace MSM_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private static JwtSettings _jwtSettings;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jwtSettingsAccesser"></param>
        public LoginController(IOptions<JwtSettings> jwtSettingsAccesser)
        {
            _jwtSettings = jwtSettingsAccesser.Value;
        }

        [HttpGet("GetToken")]
        public string GetToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey);
            var authTime = DateTime.UtcNow;//授权时间
            var expiresAt = authTime.AddHours(_jwtSettings.ExpireSeconds);//过期时间
            var tokenDescripor = new SecurityTokenDescriptor
            {
                Audience = _jwtSettings.Audience,
                Issuer = _jwtSettings.Issuer,
                Subject = new ClaimsIdentity(new Claim[] { }),
                Expires = expiresAt,
                //对称秘钥SymmetricSecurityKey
                //签名证书(秘钥，加密算法)SecurityAlgorithms
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescripor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        [HttpPost("Login")]
        [ServiceFilter(typeof(LogIActionFilterAttribute))]
        public async Task<string> Login(string info)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(info);
            if (string.IsNullOrWhiteSpace(userInfo.Account))
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "fail",
                    message = "账号不能为空",
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }

            if (string.IsNullOrWhiteSpace(userInfo.Password))
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "fail",
                    message = "密码不能为空",
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }

            LoginDal dal=new LoginDal();
            var result = await dal.Login(userInfo.Account, userInfo.Password);
            if (result.ResultCode != 0)
            {
                return JsonConvert.SerializeObject(new
                {
                    status = "fail",
                    message = result.ResultMsg,
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
            return JsonConvert.SerializeObject(new
            {
                status = "success",
                data = result.Data,
                date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Token= GetToken()
            });
        }
    }
}