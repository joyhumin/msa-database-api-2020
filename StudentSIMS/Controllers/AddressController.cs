using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentSIMS.Data;
using StudentSIMS.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace StudentSIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly StudentContext _context;

        public AddressController(StudentContext context)
        {
            _context = context;
        }

        // GET: api/Address
        [HttpGet]
        [SwaggerOperation(Summary = "Get all addresses and related student information")]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddress()
        {
            return await _context.Address.Include(c => c.Student).ToListAsync();
        }

        // GET: api/Address/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get an address by AddressId")]
        public async Task<ActionResult<Address>> GetAddress(int id)
        {
            //var address = await _context.Address.FindAsync(id);
            var address = await _context.Address.Include(s => s.Student).FirstOrDefaultAsync(i => i.AddressId == id);

            if (address == null)
            {
                return NotFound();
            }

            return address;
        }

        // PUT: api/Address/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Add an address by AddressId")]
        public async Task<IActionResult> PutAddress(int id, Address address)
        {
            if (id != address.AddressId)
            {
                return BadRequest();
            }

            _context.Entry(address).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
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

        //PUT by student id
        [HttpPut("{id}/Student/{studentid}")]
        [SwaggerOperation(Summary = "Change a student's address by StudentId")]
        public async Task<IActionResult> PutAddressbyStudent(int id,  int studentid, Address address)
        {
            if (!StudentExists(studentid))
            {
                return BadRequest();
            }
            if (id != address.AddressId)
            {
                return BadRequest();
            }

            var student = await _context.Student.FindAsync(studentid);
            address.Student = student;
            _context.Entry(address).State = EntityState.Modified;
            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
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

        // POST: api/Address
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [SwaggerOperation(Summary = "Add new address along with a student into database")]
        public async Task<ActionResult<Address>> PostAddress(Address address)
        {
            _context.Address.Add(address);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAddress", new { id = address.AddressId }, address);
        }



        // POST: api/Address/StudentId
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("Student/{studentid}")]
        [SwaggerOperation(Summary = "Add new address to an existing student")]
        public async Task<ActionResult<Address>> PostAddressbyStudent(int studentid, Address address)
        {
            //check if the student exists
            if (!StudentExists(studentid)) { return BadRequest(); }
            var student = await _context.Student.FindAsync(studentid);
            address.Student = student;
            _context.Address.Add(address);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAddress", new { id = address.AddressId }, address);
        }


        // DELETE: api/Address/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an address by AddressId")]
        public async Task<ActionResult<Address>> DeleteAddress(int id)
        {
            var address = await _context.Address.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            _context.Address.Remove(address);
            await _context.SaveChangesAsync();

            return address;
        }

        private bool AddressExists(int id)
        {
            return _context.Address.Any(e => e.AddressId == id);
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.StudentId == id);
        }
    }
}
