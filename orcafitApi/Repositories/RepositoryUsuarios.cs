﻿using orcafitApi.Data;
using orcafitApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafitApi.Repositories
{
    public class RepositoryUsuarios : IRepositoryUsuarios
    {
        //  Sentencias comunes en los repositories   ⌄⌄⌄
        private orcafitContext context;
        public RepositoryUsuarios(orcafitContext context)
        {
            this.context = context;
        }
        //  Sentencias comunes en los repositories   ˄˄˄


        private int GetMaxIdUsuario()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Usuarios.Max(z => z.IdUser) + 1;
            }
        }

        public List<Usuario> GetUsuarios()
        {
            return this.context.Usuarios.ToList();
        }
        public Usuario GetUsuario(string username)
        {
            return this.context.Usuarios.SingleOrDefault(x => x.Username.ToLower() == username.ToLower());
        }
        public int InsertUsuario(string username, string password, string imagen)
        {
            int idusuario = this.GetMaxIdUsuario();
            Usuario usuario = new Usuario();
            usuario.IdUser = idusuario;
            usuario.Username = username;
            usuario.Password = password;
            usuario.Role = "user";
            usuario.Imagen = imagen;
            usuario.Fecha = DateTime.Now;
            this.context.Usuarios.Add(usuario);
            this.context.SaveChanges();
            return idusuario;
        }
        public void DeleteUsuario(string username)
        {
            this.context.Usuarios.Remove(GetUsuario(username));
            this.context.SaveChanges();
        }
        public Usuario ExisteUsuario(string username, string password)
        {
            Usuario usuario = this.context.Usuarios.SingleOrDefault(x => x.Username.ToLower() == username.ToLower() && x.Password == password);
            return usuario;
        }

        public void UpdateImagenUsuario(int id, string imagen)
        {
            Usuario usuario = this.context.Usuarios.SingleOrDefault(x => x.IdUser == id);
            usuario.Imagen = imagen;
            this.context.SaveChanges();
        }

        public void VerificarUsuario(int id)
        {
            Usuario usuario = this.context.Usuarios.SingleOrDefault(x => x.IdUser == id);
            usuario.Verificacion = 1;
            this.context.SaveChanges();
        }
    }
}
