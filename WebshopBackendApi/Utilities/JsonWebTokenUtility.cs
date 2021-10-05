using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebshopBackendApi.Utilities
{
    public static class JsonWebTokenUtility
    {
        private static readonly JwtBuilder Jwt = JwtBuilder.Create().WithAlgorithm(new HMACSHA256Algorithm()).WithSecret("HelloWorld");
        public static string Sign(Dictionary<string, object> payload)
        {
           return Jwt.ExpirationTime(DateTime.Now.AddHours(8))
                      .AddClaims(payload)
                      .Encode();
        }

        public static string Validate(string token)
        {
           return Jwt.MustVerifySignature().Decode(token);
        }
    }
}
