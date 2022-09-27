using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebApplicationAPI.Entities;

namespace WebApplicationAPI.Models
{
    public class Curriculo
    {
        public int Id { get; set; }
        public string Experiencialaboral { get; set; }
        public string Educacion { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public int Favorito { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }

        [JsonIgnore]
        public Usuario Usuario { get; set; }

        //estos campos pueden venir de la ficha o se guardan en algun lado
        //public string NombrePersona { get; set; }
        //public int Telefono { get; set; }



        //--------EJEMPLOS DE LLAVES FORANEAS---------
        //--------------------------------------------

        //[ForeignKey("Provincia")]
        //public int ProvinciaId { get; set; }

        //[JsonIgnore]
        //public Provincia Provincia { get; set; }

    }
}
