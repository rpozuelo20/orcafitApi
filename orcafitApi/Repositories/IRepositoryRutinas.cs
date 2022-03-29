using orcafitApi.Models;
using orcafitApi.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafitApi.Repositories
{
    public interface IRepositoryRutinas
    {
        List<Categoria> GetCategorias();
        int InsertComentario(int idrutina, int iduser, string comentariotexto);
        List<ComentarioUsuarioViewModel> GetComentarios(int idrutina);
        List<Rutina> GetRutinas();
        Rutina GetRutina(int id);
        int InsertRutina(string nombre, string rutinatexto, string video, string imagen, string categoria);
        void DeleteRutina(int id);
    }
}
