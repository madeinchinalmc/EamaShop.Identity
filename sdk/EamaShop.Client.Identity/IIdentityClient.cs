﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace EamaShop.Client.Identity
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Models;

    /// <summary>
    /// The HTTP API Microservice of Identity
    /// </summary>
    public partial interface IIdentityClient : IDisposable
    {
        /// <summary>
        /// The base URI of the service.
        /// </summary>
        Uri BaseUri { get; set; }

        /// <summary>
        /// Gets or sets json serialization settings.
        /// </summary>
        JsonSerializerSettings SerializationSettings { get; }

        /// <summary>
        /// Gets or sets json deserialization settings.
        /// </summary>
        JsonSerializerSettings DeserializationSettings { get; }

        /// <summary>
        /// Subscription credentials which uniquely identify client
        /// subscription.
        /// </summary>
        ServiceClientCredentials Credentials { get; }


            /// <summary>
        /// 使用jwtBearer授权
        /// </summary>
        /// <param name='name'>
        /// 账号： 手机号/邮箱/用户名
        /// </param>
        /// <param name='password'>
        /// 用户密码
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<object>> ApiAuthorizeJwtbearerPostWithHttpMessagesAsync(string name, string password, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 获取当前的用户信息
        /// </summary>
        /// <param name='authorization'>
        /// 身份认证的授权token eg. Bearer ej
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<object>> ApiUserGetWithHttpMessagesAsync(string authorization, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 修改用户基础信息
        /// </summary>
        /// <param name='nickName'>
        /// 修改后的昵称
        /// </param>
        /// <param name='headImageUri'>
        /// 修改后的用户头像Uri地址
        /// </param>
        /// <param name='sexy'>
        /// 修改后的性别
        /// </param>
        /// <param name='authorization'>
        /// 身份认证的授权token eg. Bearer ej
        /// </param>
        /// <param name='country'>
        /// 修改后的用户所在国家
        /// </param>
        /// <param name='city'>
        /// 修改后的用户所在城市
        /// </param>
        /// <param name='province'>
        /// 修改后的用户所在省份
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> ApiUserPutWithHttpMessagesAsync(string nickName, string headImageUri, int sexy, string authorization, string country = default(string), string city = default(string), string province = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name='accountName'>
        /// 用于注册的用户名 8~18个字符之间
        /// </param>
        /// <param name='password'>
        /// 用于注册的密码 6~18个字符之间
        /// </param>
        /// <param name='headImageUri'>
        /// 头像地址的绝对路径
        /// </param>
        /// <param name='nickName'>
        /// 用户的昵称
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IDictionary<string, string>>> ApiUserPostWithHttpMessagesAsync(string accountName, string password, string headImageUri, string nickName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 修改密码接口
        /// </summary>
        /// <param name='newPassword'>
        /// 设置的新密码
        /// </param>
        /// <param name='token'>
        /// 用户修改密码的凭证 短信为验证码，邮箱也为验证码
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> ApiUserPasswordPutWithHttpMessagesAsync(string newPassword, string token, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 绑定手机号码
        /// </summary>
        /// <param name='phone'>
        /// 手机号码
        /// </param>
        /// <param name='verifyCode'>
        /// 验证码
        /// </param>
        /// <param name='authorization'>
        /// 身份认证的授权token eg. Bearer ej
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> ApiUserPhonePutWithHttpMessagesAsync(string phone, string verifyCode, string authorization, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 修改用户角色为商户 该接口不会对外提供，只能在当前测试页面进行查看
        /// </summary>
        /// <param name='id'>
        /// </param>
        /// <param name='authorization'>
        /// 身份认证的授权token eg. Bearer ej
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> ApiUserRoleByIdPutWithHttpMessagesAsync(long id, string authorization, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 发送验证码给指定的手机号
        /// 接口未实现，默认使用验证码123456
        /// </summary>
        /// <param name='phone'>
        /// 用户的手机号码
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> ApiVerifycodePhonePostWithHttpMessagesAsync(string phone = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 发送邮箱验证码到指定邮箱
        /// 接口未实现，默认使用验证码123456
        /// </summary>
        /// <param name='email'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> ApiVerifycodeEmailPostWithHttpMessagesAsync(string email = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

    }
}