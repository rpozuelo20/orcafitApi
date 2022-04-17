using Microsoft.EntityFrameworkCore;
using orcafitApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafitApi.Data
{
    public class orcafitContext : DbContext
    {
        public orcafitContext(DbContextOptions<orcafitContext> options) : base(options) { }

        //  Inyeccion de los modelos:
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Rutina> Rutinas { get; set; }
    }
}
