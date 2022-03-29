﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace orcafitApi.Models
{
    [Table("USUARIOS")]
    public class Usuario
    {
        [Key]
        [Column("USER_NO")]
        public int IdUser { get; set; }
        [Column("USERNAME")]
        public string Username { get; set; }
        [Column("PASSWORD")]
        public string Password { get; set; }
        [Column("ROLE")]
        public string Role { get; set; }
        [Column("IMAGEN")]
        public string Imagen { get; set; }
        [Column("FECHA")]
        public DateTime Fecha { get; set; }
    }
}
