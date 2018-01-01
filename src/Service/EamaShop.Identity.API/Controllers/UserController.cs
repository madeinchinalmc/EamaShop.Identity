using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EamaShop.Identity.API.Dto;
using Microsoft.Extensions.DependencyInjection;
using EamaShop.Identity.Services;
using System.Security.Claims;
using System.Net;
using System.ComponentModel.DataAnnotations;

namespace EamaShop.Identity.API.Controllers
{
    /// <summary>
    /// 用户相关接口
    /// </summary>
    [Produces("application/json")]
    [Route("api/User")]
    [Authorize]
    public class UserController : Controller
    {
        /// <summary>
        /// 用户注册接口
        /// </summary>
        /// <param name="parameters">登陆的参数信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ResultDTO))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResultDTO))]
        public async Task<IActionResult> Register([FromBody]UserRegisterDTO parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResultDTO.New(ModelState.ToResponseString()));
            }
            var service = HttpContext.RequestServices.GetRequiredService<IRegisterService>();

            await service.RegisterAsync(
                account: parameters.AccountName,
                 password: parameters.Password,
                 cancellationToken: HttpContext.RequestAborted,
                 headImageUri: parameters.HeadImageUri,
                 nickName: parameters.NickName);

            return Ok(ResultDTO.New());
        }

        /// <summary>
        /// 获取当前的用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResultDTO<UserInfoDTO>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ResultDTO<UserInfoDTO>))]
        public async Task<IActionResult> Details()
        {
            var userInfoService = HttpContext
                .RequestServices
                .GetRequiredService<IUserInfoService>();

            var user = await userInfoService.GetByIdAsync(User.GetId(), HttpContext.RequestAborted);

            if (user == null)
            {
                return NotFound(ResultDTO.New("用户不存在"));
            }

            return Ok(ResultDTO.Ok(new UserInfoDTO(user)));
        }
        /// <summary>
        /// 修改用户基础信息
        /// </summary>
        /// <param name="parameters">用户的修改信息上下文参数</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResultDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ResultDTO))]
        public async Task<IActionResult> Put([FromForm]UserPutDTO parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResultDTO.New(ModelState.ToResponseString()));
            }

            var userInfoService = HttpContext
                .RequestServices
                .GetRequiredService<IUserInfoService>();

            await userInfoService.EditInfo(HttpContext.User.GetId(), (editor) =>
             {
                 // TODO : is it neccessary using AutoMapper instead?
                 editor.City = parameters.City;
                 editor.Country = parameters.Country;
                 editor.NickName = parameters.NickName;
                 editor.Province = parameters.Province;
                 editor.Sexy = parameters.Sexy;
                 editor.HeadImageUri = parameters.HeadImageUri;
             }, HttpContext.RequestAborted);

            return Ok(ResultDTO.New());
        }
        /// <summary>
        /// 修改密码接口
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut("password")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResultDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ResultDTO))]
        public async Task<IActionResult> ChangePassword([FromForm]UserPasswordPutDTO parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResultDTO.New(ModelState.ToResponseString()));
            }

            var userInfoService = HttpContext
                .RequestServices
                .GetRequiredService<IUserInfoService>();

            await userInfoService
                 .ChangePasswordAsync(
                id: HttpContext.User.GetId(),
                password: parameters.NewPassword,
                token: parameters.Token,
                cancellationToken: HttpContext.RequestAborted);

            return Ok(ResultDTO.New());
        }
        /// <summary>
        /// 绑定手机号码
        /// </summary>
        /// <returns></returns>
        [HttpPut("phone")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResultDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ResultDTO))]
        public async Task<IActionResult> BindPhone([FromForm]UserPhonePutDTO parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResultDTO.New(ModelState.ToResponseString()));
            }

            var userInfoService = HttpContext
                .RequestServices
                .GetRequiredService<IUserInfoService>();

            await userInfoService
                 .BindPhone(
                id: HttpContext.User.GetId(),
                phone: parameters.Phone,
                verifyCode: parameters.VerifyCode,
                cancellationToken: HttpContext.RequestAborted);

            return Ok(ResultDTO.New());
        }
        /// <summary>
        /// 修改用户角色为商户 该接口不会对外提供，只能在当前测试页面进行查看
        /// </summary>
        /// <returns></returns>
        [HttpPut("role/{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ResultDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ResultDTO))]
        public async Task<IActionResult> Role([Range(1, long.MaxValue)]long id)
        {
            var userInfoService = HttpContext
               .RequestServices
               .GetRequiredService<IUserInfoService>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ResultDTO.New(ModelState.ToResponseString()));
            }

            await userInfoService.ChangeRole(id, UserRole.Merchant);

            return Ok(ResultDTO.New());
        }
    }
}