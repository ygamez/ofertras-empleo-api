using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationAPI.Data;
using WebApplicationAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationAPI.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class tb_ma_dpasController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public tb_ma_dpasController( ApplicationDbContext context )
        {
            _context = context;
        }

        //// GET: api/<tb_ma_dpasController>
        ////[HttpGet]
        //public async Task<ActionResult<IEnumerable<tb_ma_dpas>>> GetDpas()
        //{
        //    return await _context.tb_Ma_Dpas.ToListAsync();
        //}

        // GET api/<tb_ma_dpasController>/5
        //[HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<tb_ma_dpasController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<tb_ma_dpasController>/5
        //[HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<tb_ma_dpasController>/5
        //[HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
