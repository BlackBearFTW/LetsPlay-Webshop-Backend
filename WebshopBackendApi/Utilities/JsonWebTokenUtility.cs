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
        public static string Sign(Dictionary<string, object> payload)
        {
            return jwtBuilderGenerator()
                      .ExpirationTime(DateTime.Now.AddHours(8))
                      .AddClaims(payload)
                      .Encode();
        }

        public static string Validate(string token)
        {
           return jwtBuilderGenerator().MustVerifySignature().Decode(token);
        }

        private static JwtBuilder jwtBuilderGenerator()
        {
            return JwtBuilder.Create().WithAlgorithm(new HMACSHA256Algorithm()).WithSecret("HelloWorld");
        }
    }
}
