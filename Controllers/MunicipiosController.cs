using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPI.Data;
using WebApplicationAPI.Models;

namespace WebApplicationAPI.Controllers
{
    [Route("api/Provincia/{ProvinciaId}/Municipios")]
    [ApiController]
    public class MunicipiosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MunicipiosController(ApplicationDbContext context)
        {
            _context = context;
        }

        //// GET: api/Municipios
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Municipio>>> GetMunicipio(int id)
        //{
        //    return await _context.Municipios.Where(x => x.ProvinciaId == id).ToListAsync();
        //}

        // GET: api/Municipios
        [HttpGet]
        public IEnumerable<Municipio> GetMcpio(int ProvinciaId)
        {
            return _context.Municipios.Where(x => x.ProvinciaId == ProvinciaId).ToList();
        }

        //get: api/municipios/5
        [HttpGet("{id}", Name ="municiioById" )]
        public async Task<ActionResult<Municipio>> getmunicipioxid(int id)
        {
            var municipio = await _context.Municipios.FindAsync(id);

            if (municipio == null)
            {
                return NotFound();
            }

            return new ObjectResult(municipio);
        }

        // PUT: api/Municipios/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMunicipio(int id, Municipio municipio)
        {
            if (id != municipio.Id)
            {
                return BadRequest();
            }

            _context.Entry(municipio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MunicipioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Municipios
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Municipio>> PostMunicipio(Municipio municipio, int idProvincia)
        {
            municipio.ProvinciaId = idProvincia;
            _context.Municipios.Add(municipio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostMunicipio", new { id = municipio.Id }, municipio );
        }

        // DELETE: api/Municipios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Municipio>> DeleteMunicipio(int id)
        {
            var municipio = await _context.Municipios.FindAsync(id);
            if (municipio == null)
            {
                return NotFound();
            }

            _context.Municipios.Remove(municipio);
            await _context.SaveChangesAsync();

            return municipio;
        }

        private bool MunicipioExists(int id)
        {
            return _context.Municipios.Any(e => e.Id == id);
        }
    }
}
