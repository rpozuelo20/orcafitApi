using orcafitApi.Data;
using orcafitApi.Models;
using orcafitApi.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafitApi.Repositories
{
    public class RepositoryRutinas : IRepositoryRutinas
    {
        //  Sentencias comunes en los repositories   ⌄⌄⌄
        private orcafitContext context;
        public RepositoryRutinas(orcafitContext context)
        {
            this.context = context;
        }
        //  Sentencias comunes en los repositories   ˄˄˄


        private int GetMaxIdRutina()
        {
            if (this.context.Rutinas.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Rutinas.Max(z => z.IdRutina) + 1;
            }
        }
        private int GetMaxIdComentario()
        {
            if (this.context.Comentarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Comentarios.Max(z => z.IdComentario) + 1;
            }
        }
        public List<Categoria> GetCategorias()
        {
            var consulta = from datos in this.context.Categorias
                           select datos;
            return consulta.ToList();
        }
        public List<ComentarioUsuarioViewModel> GetComentarios(int idrutina)
        {
            var consulta = from comentario in this.context.Comentarios
                           join usuario in this.context.Usuarios
                           on comentario.IdUser equals usuario.IdUser
                           where comentario.IdRutina == idrutina
                           orderby comentario.IdComentario descending
                           select new ComentarioUsuarioViewModel
                           {
                               IdComentario = comentario.IdComentario,
                               ComentarioTexto = comentario.ComentarioTexto,
                               Fecha = comentario.Fecha,
                               Username = usuario.Username,
                               Imagen = usuario.Imagen
                           };
            return consulta.ToList();
        }
        public int InsertComentario(int idrutina, int iduser, string comentariotexto)
        {
            int idcomentario = this.GetMaxIdComentario();
            Comentario comentario = new Comentario();
            comentario.IdComentario = idcomentario;
            comentario.IdRutina = idrutina;
            comentario.IdUser = iduser;
            comentario.ComentarioTexto = comentariotexto;
            comentario.Fecha = DateTime.Now;
            this.context.Comentarios.Add(comentario);
            this.context.SaveChanges();
            return idcomentario;
        }
        public List<Rutina> GetRutinas()
        {
            var consulta = from datos in this.context.Rutinas
                           select datos;
            return consulta.ToList();
        }
        public Rutina GetRutina(int id)
        {
            return this.context.Rutinas.SingleOrDefault(x => x.IdRutina == id);
        }
        public int InsertRutina(string nombre, string rutinatexto, string video, string imagen, string categoria)
        {
            int idrutina = this.GetMaxIdRutina();
            Rutina rutina = new Rutina();
            rutina.IdRutina = idrutina;
            rutina.Nombre = nombre;
            rutina.RutinaTexto = rutinatexto;
            rutina.Video = video;
            rutina.Imagen = imagen;
            rutina.Categoria = categoria;
            rutina.Fecha = DateTime.Now;
            rutina.Tier = "free";

            this.context.Rutinas.Add(rutina);
            this.context.SaveChanges();

            return idrutina;
        }
        public void DeleteRutina(int id)
        {
            this.context.Rutinas.Remove(GetRutina(id));
            this.context.SaveChanges();
        }
    }
}
