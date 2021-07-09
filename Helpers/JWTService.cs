using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System;
using Microsoft.AspNetCore.Http;

namespace summerSemesterProj.Helpers {
    public class JWTService {
        //  creating JWT token string 
        
        private string secureKey = "qwertyuiopasdfghjklzxcvbnm";
        public string generate(string id){
            var symSecKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
            var credentials = new SigningCredentials(symSecKey, SecurityAlgorithms.HmacSha256Signature);
            var headerJwt = new JwtHeader(credentials);
            
            // data we want to encode
            var payload = new JwtPayload(id, null, null, null, DateTime.Today.AddMonths(1));
            var secToken = new JwtSecurityToken(headerJwt, payload);

            return new JwtSecurityTokenHandler().WriteToken(secToken); 
        }

        public JwtSecurityToken verifyToken(string token){
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secureKey);
            tokenHandler.ValidateToken(token, new TokenValidationParameters{
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true, 
                ValidateIssuer = false,
                ValidateAudience = false
            }
            , out SecurityToken validatedToken);

            return (JwtSecurityToken) validatedToken;
        }

        public string validateToken(HttpRequest Request){
            var jwtToken = Request.Cookies["jwtToken"];
            if(jwtToken == null){
                return null;
            };
            
            var token = verifyToken(jwtToken);
            string userId = token.Issuer;

            return userId;
        }
    }
}