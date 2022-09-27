using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationAPI.Models
{
    public class Provincia
    {
        public Provincia()
        {
            Municipios = new List<Municipio>();
        }
        public int Id { get; set; }
        [StringLength(30)]
        public string provincia { get; set; }

        public List<Municipio> Municipios { get; set; }
    }
}
