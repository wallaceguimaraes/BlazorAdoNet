using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebApi.Models.Entities.Auth;

namespace WebApi.Extensions.Identity
{
    public static class ClaimsExtensions
    {
        public static string Actor(this IEnumerable<Claim> claims)
        {
            var claim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Actor);
            return claim?.Value;
        }

        public static long UserId(this IEnumerable<Claim> claims)
        {
            var claim = claims.FirstOrDefault(c => c.Type == ApiClaimTypes.UserId);
            var userId = 0L;

            Int64.TryParse(claim?.Value, out userId);

            return userId;
        }

        public static string Salt(this IEnumerable<Claim> claims)
        {
            var claim = claims.FirstOrDefault(c => c.Type == ApiClaimTypes.Salt);

            return claim?.Value;
        }

        public static string ClientId(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        }
    }
}