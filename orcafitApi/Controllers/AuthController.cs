using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using orcafitApi.Helpers;
using orcafitApi.Models;
using orcafitApi.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace orcafitApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //  Sentencias comunes en los controllers   ⌄⌄⌄
        private IRepositoryUsuarios repo;
        private HelperOAuthToken helper;
        public AuthController(IRepositoryUsuarios repo, HelperOAuthToken helper)
        {
            this.repo = repo;
            this.helper = helper;
        }
        //  Sentencias comunes en los controllers   ˄˄˄

        // Metodo de validacion de usuario-contraseña
        [HttpPost]
        [Route("[action]")]
        public ActionResult Login(LoginModel model)
        {
            Usuario usuario = this.repo.ExisteUsuario(model.Username, model.Password);
            if (usuario == null)
            {
                // Si el usuario no coincide entonces nos devolvera un no autorizado
                return Unauthorized();
            }
            else
            {
                // Si el usuario coincide entonces nos guardamos las credenciales del Token
                SigningCredentials credentials = new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);
                // Dentro del Token podemos almacenar cualquier informacion en formato JSON mediante un array llamado Claims
                string jsonUsuario = JsonConvert.SerializeObject(usuario);
                Claim[] claims = new[] { new Claim("UserData", jsonUsuario) };
                // Generamos el Token, que estara compuesto por el Issuer, Audience, Credentials, Time...
                JwtSecurityToken token = new JwtSecurityToken(
                        claims: claims,
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        notBefore: DateTime.UtcNow
                        );
                // Devolvemos una respuesta Ok con el Token generado
                return Ok(new { response = new JwtSecurityTokenHandler().WriteToken(token) });
            }
        }
    }
}
