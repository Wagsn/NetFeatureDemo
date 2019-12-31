using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthDemo
{
    /// <summary>
    /// 自定义的Token验证器
    /// </summary>
    public class MyTokenValidator : ISecurityTokenValidator
    {
        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; } = 1024;

        public bool CanReadToken(string securityToken)
        {
            return !string.IsNullOrWhiteSpace(securityToken);
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            validatedToken = null;
            if(securityToken != "abcdefg")
            {
                return new ClaimsPrincipal();
            }
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim("name", "jim"));
            identity.AddClaim(new Claim("SuperAdminOnly", "true"));
            identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, "user"));
            var principal = new ClaimsPrincipal(identity);

            return principal;
        }
    }
}
