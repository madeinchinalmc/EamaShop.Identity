using System;
using System.Collections.Generic;
using System.Text;

namespace EamaShop.Identity.Services
{
    /// <summary>
    /// 用户登陆的授权token信息
    /// </summary>
    public struct UserToken
    {
        /// <summary>
        /// initialize a new <see cref="UserToken"/> instance.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="expiredIn"></param>
        public UserToken(string token, DateTime expiredIn) : this()
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
            ExpiredIn = expiredIn;
        }
        /// <summary>
        /// 用户访问的授权token
        /// </summary>
        public string Token { get; }
        /// <summary>
        /// Gets or sets the expired absolute time.
        /// </summary>
        public DateTime ExpiredIn { get; }
        /// <summary>
        /// Get Token Content.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Token;
        }
    }
}
