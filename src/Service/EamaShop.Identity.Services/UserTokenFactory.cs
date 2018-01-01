using System;
using System.Collections.Generic;
using System.Text;
using EamaShop.Identity.DataModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Linq;
using Newtonsoft.Json;
using EamaShop.Infrastructures.BLLModels;

namespace EamaShop.Identity.Services
{
    public class UserTokenFactory : IUserTokenFactory
    {
        public UserToken CreateToken(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var identity = TransformAsClaimIdentity(user);

            var securityTokenDescriptor = CreateDescriptor(identity);

            var tokenHandler = new JwtSecurityTokenHandler();

            var stoken = tokenHandler.CreateJwtSecurityToken(securityTokenDescriptor);

            var result = tokenHandler.WriteToken(stoken);

            return new UserToken(result, securityTokenDescriptor.Expires.Value);
        }

        protected virtual ClaimsIdentity TransformAsClaimIdentity(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var defitions = Enum.GetValues(typeof(UserRole)).Cast<UserRole>().ToArray();

            var roles = Array
                .FindAll(defitions, x => user.Role.HasFlag(x))
                .Select(x=>x.ToString())
                .ToArray();

            var eamaUser = new EamaUser(user.Id, user.AccountName, roles);
            return eamaUser.GetPrincipal().Identities.OrderByDescending(x => x.FindFirstValue<UserRole>(ClaimsIdentity.DefaultRoleClaimType)).First();
        }

        private Claim Create(string type, string value, string dataType)
        {
            return new Claim(type, value, dataType, ClaimsIdentity.DefaultIssuer);
        }

        private SecurityTokenDescriptor CreateDescriptor(ClaimsIdentity identity)
        {
            var signKeyBytes = Encoding.ASCII.GetBytes(EamaDefaults.JwtBearerSignKey);
            var sskey = new SymmetricSecurityKey(signKeyBytes);
            var cre = new SigningCredentials(sskey, SecurityAlgorithms.HmacSha256Signature);

            var tokenKeyBytes = Encoding.ASCII.GetBytes(EamaDefaults.JwtBearerTokenKey);
            var tokenKey = new SymmetricSecurityKey(tokenKeyBytes);
            var tokenCre = new EncryptingCredentials(tokenKey,
                SecurityAlgorithms.Aes128KW,
                SecurityAlgorithms.Aes128CbcHmacSha256);

            return new SecurityTokenDescriptor()
            {
                Subject = identity,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = cre,
                Audience = EamaDefaults.Audience,
                Issuer = ClaimsIdentity.DefaultIssuer,
                EncryptingCredentials = tokenCre
            };
        }


    }
}
