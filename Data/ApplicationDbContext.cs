using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationAPI.Entities;
using WebApplicationAPI.Models;

namespace WebApplicationAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet< Provincia > Provincias { get; set; }
        public DbSet< Municipio > Municipios { get; set; }
        public DbSet< Usuario > Usuarios { get; set; }
        public DbSet< Curriculo > Curriculos { get; set; }
    }
}
