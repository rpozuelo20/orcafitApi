using orcafitApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafitApi.Repositories
{
    public interface IRepositoryUsuarios
    {
        List<Usuario> GetUsuarios();
        Usuario GetUsuario(string username);
        int InsertUsuario(string username, string password, string imagen);
        void DeleteUsuario(string username);
        Usuario ExisteUsuario(string username, string password);
        void UpdateImagenUsuario(int id, string imagen);
        void VerificarUsuario(int id);
    }
}