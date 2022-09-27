using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationAPI.Models
{
    public class tb_ma_dpas
    {
        [Key]
        public int id_dpa { get; set; }
        public string dpa_nombre { get; set; }
        public int hijode { get; set; }
        public int estado { get; set; }
        public int est_replica { get; set; }
        public string description { get; set; }
    }
}
