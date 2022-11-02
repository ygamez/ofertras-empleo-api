using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPI.Data;
using WebApplicationAPI.Models;
using WebApplicationAPI.Models.Users;

namespace WebApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculoesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CurriculoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Curriculoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curriculo>>> GetCurriculos()
        {
            return await _context.Curriculos.ToListAsync();
        }

        // GET: api/Curriculoes/xusuario/:5
        [HttpGet("xusuario/{usuarioId}")]
        public IEnumerable<Curriculo> GetCurriculoxUsuario(int usuarioId)
        {
            return _context.Curriculos.Where( x=> x.UsuarioId == usuarioId).ToList() ;
        }


        // GET: api/Curriculoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Curriculo>> GetCurriculo(int id)
        {
            var curriculo = await _context.Curriculos.FirstOrDefaultAsync(x => x.Id == id);

            if (curriculo == null)
            {
                return NotFound();
            }

            return new ObjectResult( curriculo );
        }

        // PUT: api/Curriculoes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurriculo(int id, Curriculo curriculo)
        {
            if (id != curriculo.Id)
            {
                return BadRequest();
            }

            _context.Entry(curriculo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CurriculoExists(id))
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

        // POST: api/Curriculoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Curriculo>> PostCurriculo(Curriculo curriculo )
        {
            var curriculoexis = await _context.Curriculos.FindAsync(curriculo.Id );

            if (CurriculoUsuario(curriculo.UsuarioId))
            {
                return BadRequest(new { ok = "false", message = "ya existe un curriculo para este usuario" });
            }

            //if (_context.Curriculos.Any(x => x.UsuarioId == curriculo.))
            //    throw new AppException("Username \"" + user.Nombre + "\" is already taken");

            if (curriculoexis == null)
            {
                _context.Curriculos.Add(curriculo);
                await _context.SaveChangesAsync();  

            }
            return CreatedAtAction("GetCurriculo", new { id = curriculo.Id }, curriculo);

            //return BadRequest(new {  message = "error ya existe" });
        }        

        // DELETE: api/Curriculoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Curriculo>> DeleteCurriculo(int id)
        {
            var curriculo = await _context.Curriculos.FindAsync(id);
            if (curriculo == null)
            {
                return NotFound();
            }

            _context.Curriculos.Remove(curriculo);
            await _context.SaveChangesAsync();

            return curriculo;
        }

        private bool CurriculoExists(int id)
        {
            return _context.Curriculos.Any(e => e.Id == id);
        }

        private bool CurriculoUsuario(int usuarioId)
        {
            return _context.Curriculos.Any( c => c.UsuarioId == usuarioId );
        }
    }
}
