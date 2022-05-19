using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using orcafitApi.Helpers;
using orcafitApi.Models;
using orcafitApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace orcafitApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        //  Sentencias comunes en los controllers   ⌄⌄⌄
        private IRepositoryUsuarios repo;
        private HelperOAuthToken helper;
        public UsuariosController(IRepositoryUsuarios repo, HelperOAuthToken helper)
        {
            this.repo = repo;
            this.helper = helper;
        }
        //  Sentencias comunes en los controllers   ˄˄˄


        //  Metodo en el controller para recuperar los datos de un usuario:
        [HttpGet]
        [Authorize]
        [Route("[action]")]
        public ActionResult<Usuario> GetPerfilUsuario()
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            string jsonUser = claims.SingleOrDefault(z => z.Type == "UserData").Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(jsonUser);
            return usuario;
        }
        //  Metodo en el controller para recuperar una lista de usuarios:
        [HttpGet]
        public ActionResult<List<Usuario>> GetUsuarios()
        {
            List<Usuario> usuarios = this.repo.GetUsuarios();
            return usuarios;
        }
        //  Metodo en el controller para recuperar un usuario:
        [HttpGet("{username}")]
        public ActionResult<Usuario> GetUsuario(string username)
        {
            Usuario usuario = this.repo.GetUsuario(username);
            return usuario;
        }
        //  Metodo en el controller para insertar un usuario:
        [HttpPost]
        public void InsertUsuario(Usuario usuario)
        {
            this.repo.InsertUsuario(usuario.Username, usuario.Password, usuario.Imagen);
        }
        //  Metodo en el controller para borrar un usuario:
        [HttpDelete]
        [Authorize]
        public void DeleteUsuario()
        {
            Usuario usuario = this.helper.GetUserFromJwt(HttpContext.User.Claims.ToList());
            this.repo.DeleteUsuario(usuario.Username);
        }
        [HttpPut]
        [Authorize]
        public void UpdateImagenUsuario(Usuario usuario)
        {
            Usuario currentUser = this.helper.GetUserFromJwt(HttpContext.User.Claims.ToList());
            this.repo.UpdateImagenUsuario(currentUser.IdUser, usuario.Imagen);
        }
        [HttpPut]
        [Route("[action]")]
        public void VerificarUsuario(Usuario usuario)
        {
            this.repo.VerificarUsuario(usuario.IdUser);
        }
    }
}
