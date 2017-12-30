using EamaShop.Identity.API.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EamaShop.Identity.API.Dto
{
    /// <summary>
    /// 用户登陆接口参数 <see cref="AuthorizeController.JwtBearer(JwtBearerAuthDto)"/>
    /// </summary>
    public class JwtBearerAuthDto
    {
        /// <summary>
        /// 账号： 手机号/邮箱/用户名
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
