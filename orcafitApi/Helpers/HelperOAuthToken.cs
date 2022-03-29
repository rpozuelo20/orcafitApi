using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using orcafitApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace orcafitApi.Helpers
{
    public class HelperOAuthToken
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public HelperOAuthToken(IConfiguration configuration)
        {
            this.Issuer = configuration.GetValue<string>("ApiOAuth:Issuer");
            this.Audience = configuration.GetValue<string>("ApiOAuth:Audience");
            this.SecretKey = configuration.GetValue<string>("ApiOAuth:SecretKey");
        }

        // Metodo que genera el Token mediante una clave simetrica mediante un SecretKey personalizado con cifrado
        public SymmetricSecurityKey GetKeyToken()
        {
            byte[] data =
                Encoding.UTF8.GetBytes(this.SecretKey);
            return new SymmetricSecurityKey(data);
        }
        // Configuracion de las opciones para la realizacion de la validacion de nuestro Token
        public Action<JwtBearerOptions> GetJwtOptions()
        {
            Action<JwtBearerOptions> options =
                new Action<JwtBearerOptions>(options =>
                {
                options.TokenValidationParameters =
                    new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateActor = true,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidIssuer = this.Issuer,
                        ValidAudience = this.Audience,
                        IssuerSigningKey = this.GetKeyToken()
                    };
                });
            return options;
        }
        // Metodo que genera nustro esquema de autentificacion
        public Action<AuthenticationOptions> GetAuthenticationOptions()
        {
            Action<AuthenticationOptions> options =
                new Action<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                });
            return options;
        }

        /// <summary>
        /// Metodo para recuperar el usuario token, esta funcion solamente recupera el usuario en el controlador.
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public Usuario GetUserFromJwt(List<Claim> claims)
        {
            string jsonUsuario = claims.SingleOrDefault(z => z.Type == "UserData").Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(jsonUsuario);
            return usuario;
        }
    }
}
