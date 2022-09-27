using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApplicationAPI.Models
{
    public class Municipio
    {
        public int Id { get; set; }
        //public string municipio { get; set; }
        public string municipio { get; set; }
        [ForeignKey("Provincia")]
        public int ProvinciaId { get; set; }

        [JsonIgnore]
        public Provincia Provincia { get; set; }
    }
}
