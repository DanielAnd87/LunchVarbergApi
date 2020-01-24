using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAuth.Data;
using TestAuth.Models;

namespace TestAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerContext _context;

        public CustomersController(CustomerContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
        {
            return await _context.Customer.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {

            var customer = await _context.Customer.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        private bool IsCorrectUser(int customerId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            int userId = Convert.ToInt32(claim[2].Value);
            bool isAdmin = claim[3].Value == "1" ? true : false;
            if (!isAdmin | userId != customerId)
            {
                return false;
            }else
            {
                return true;
            }
        }

        // PUT: api/Customers/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id,[FromBody] Customer customer)
        {
            if (!IsCorrectUser(customer.Id))
            {
                return Unauthorized("That's not your account!");
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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
        
            // POST: api/Customers
            [HttpPost("post")]
        public async Task<ActionResult<Customer>> PostCustomer([FromBody] Customer customer) {
            if (!IsCorrectUser(customer.Id))
            {
                return Unauthorized("That's not your account!");
            }
            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            if (!IsCorrectUser(id))
            {
                return Unauthorized("That's not your account!");
            }
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }
    }
}
