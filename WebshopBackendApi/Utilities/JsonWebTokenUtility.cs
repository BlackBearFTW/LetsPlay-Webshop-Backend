using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebshopBackendApi.Utilities
{
    public static class JsonWebTokenUtility
    {
        public static string Sign(Claim[] payload)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("9oXCULT4$9jherZsbU&d&^YzKw4k5a");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(payload),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            Console.WriteLine(token);
            return tokenHandler.WriteToken(token);
        }

/*        public static string Validate(string token)
        {
            return jwtBuilderGenerator().MustVerifySignature().Decode(token);
        }

        private static JwtBuilder jwtBuilderGenerator()
        {
            return JwtBuilder.Create().WithAlgorithm(new HMACSHA256Algorithm()).WithSecret("HelloWorld");
        }*/
    }
}
