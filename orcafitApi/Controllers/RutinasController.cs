using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using orcafitApi.Helpers;
using orcafitApi.Models;
using orcafitApi.Models.ViewModels;
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
    public class RutinasController : ControllerBase
    {
        //  Sentencias comunes en los controllers   ⌄⌄⌄
        private IRepositoryRutinas repo;
        private HelperOAuthToken helper;
        public RutinasController(IRepositoryRutinas repo, HelperOAuthToken helper)
        {
            this.repo = repo;
            this.helper = helper;
        }
        //  Sentencias comunes en los controllers   ˄˄˄


        //  Metodo para recuperar las categorias:
        [HttpGet]
        [Route("Categorias")]
        public ActionResult<List<Categoria>> GetCategorias()
        {
            return this.repo.GetCategorias();
        }
        //  Metodo para recuperar los comentarios:
        [HttpGet]
        [Route("Comentarios/"+"{idrutina}")]
        [Authorize]
        public ActionResult<List<ComentarioUsuarioViewModel>> GetComentarios(int idrutina)
        {
            return this.repo.GetComentarios(idrutina);
        }
        //  Metodo para insertar un comentario:
        [HttpPost]
        [Route("Comentarios/"+"{idrutina}")]
        [Authorize]
        public ActionResult InsertComentario(int idrutina, Comentario comentario)
        {
            Usuario usuario = this.helper.GetUserFromJwt(HttpContext.User.Claims.ToList());
            this.repo.InsertComentario(idrutina, usuario.IdUser, comentario.ComentarioTexto);
            return Ok();
        }
        //  Metodo para recuperar las rutinas:
        [HttpGet]
        public ActionResult<List<Rutina>> GetRutinas()
        {
            return this.repo.GetRutinas();
        }
        //  Metodo para recuperar una rutina:
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<Rutina> GetRutina(int id)
        {
            Usuario usuario = this.helper.GetUserFromJwt(HttpContext.User.Claims.ToList());
            return this.repo.GetRutina(id);
        }
        //  Metodo para insertar una rutina:
        [HttpPost]
        [Authorize]
        public ActionResult InsertRutina(Rutina rutina)
        {
            Usuario usuario = this.helper.GetUserFromJwt(HttpContext.User.Claims.ToList());
            if (usuario.Role == "admin")
            {
                this.repo.InsertRutina(rutina.Nombre, rutina.RutinaTexto, rutina.Video, rutina.Imagen, rutina.Categoria);
                return Ok();
            }
            else
            {
                return Unauthorized("Error, no dispone de permisos suficientes para poder realizar la siguiente acción.");
            }
        }
        //  Metodo para borrar una rutina:
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeleteRutina(int id)
        {
            Usuario usuario = this.helper.GetUserFromJwt(HttpContext.User.Claims.ToList());
            if (usuario.Role == "admin")
            {
                this.repo.DeleteRutina(id);
                return Ok();
            } else
            {
                return Unauthorized("Error, no dispone de permisos suficientes para poder realizar la siguiente acción.");
            }
        }
    }
}
