using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPI.Data;
using WebApplicationAPI.Models;

namespace WebApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class ProvinciasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProvinciasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Provincias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Provincia>>> GetProvincias()
        {
            return await _context.Provincias.ToListAsync();
        }

        //// GET: api/Provincias
        //[HttpGet]
        //public IEnumerable<Provincia> GetDpas()
        //{
        //    return _context.Provincias.ToList();
        //}

        // GET: api/Provincias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Provincia>> GetProvincia(int id)
        {
            var provincia = await _context.Provincias.Include(x => x.Municipios).FirstOrDefaultAsync(x=> x.Id == id);

            if (provincia == null)
            {
                return NotFound();
            }

            return Ok( provincia );
        }

        // PUT: api/Provincias/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProvincia(int id, Provincia provincia)
        {
            if (id != provincia.Id)
            {
                return BadRequest();
            }

            _context.Entry(provincia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProvinciaExists(id))
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

        // POST: api/Provincias
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Provincia>> PostProvincia(Provincia provincia)
        {
            if (ModelState.IsValid)
            {
                _context.Provincias.Add(provincia);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetProvincia", new { id = provincia.Id }, provincia);
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Provincias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Provincia>> DeleteProvincia(int id)
        {
            var provincia = await _context.Provincias.FindAsync(id);

            if (provincia == null)
            {
                return NotFound();
            }

            _context.Provincias.Remove(provincia);
            await _context.SaveChangesAsync();

            return provincia;
        }

        private bool ProvinciaExists(int id)
        {
            return _context.Provincias.Any(e => e.Id == id);
        }
    }
}
