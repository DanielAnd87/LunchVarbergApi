using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAuth.Data;
using TestAuth.Models;

namespace TestAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly MenuContext _context;

        public MenusController(MenuContext context)
        {
            _context = context;
        }

        // GET: api/Menus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Menu>>> GetMenu()
        {

            List<Menu> menues = await _context.Menu.ToListAsync();
            return menues;
        }
        /// <summary>
        /// Gets the daily menues for choosen resturant. 
        /// </summary>
        /// <param name="id">The resturant id.</param>
        /// <returns></returns>
        // GET: api/Menus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Menu>>> GetMenu(int id)
        {
            /*
            var menu = await _context.Menu.FindAsync(id);

            if (menu == null)
            {
                return NotFound();
            }

            return menu;
            */



            List<Menu> menues = await _context.Menu.ToListAsync();
            List<Menu> resturantMenus = new List<Menu>();
            foreach (Menu menu in menues)
            {
                if (menu.CustomerId == id)
                {
                    resturantMenus.Add(menu);
                }
            }


            
            return resturantMenus;
        }

        // PUT: api/Menus/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMenu(int id,[FromBody] Menu menu)
        {
            if (IsCorrectUser(menu.CustomerId))
            {
                return Unauthorized("DENIED");
            }
            if (id != menu.Id)
            {
                return BadRequest();
            }
            _context.Entry(menu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuExists(id))
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

        // POST: api/Menus
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Menu>> PostMenu(Menu menu)
        {
            if (!IsCorrectUser(menu.CustomerId))
            {
                return Unauthorized("DENIED");
            }

            _context.Menu.Add(menu);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMenu", new { id = menu.Id }, menu);
        }

        // DELETE: api/Menus/5
        //[HttpDelete("{id}")]
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<Menu>> DeleteMenu(int id)
        {
            List<Menu> menues = await _context.Menu.ToListAsync();
            List<Menu> resturantMenus = new List<Menu>();
            foreach (Menu item in menues)
            {
                if (item.Id == id)
                {
                    resturantMenus.Add(item);
                }
            }
            if (resturantMenus.Count == 0)
            {
                return NoContent();
            }


            //var identity = HttpContext.User.Identity as ClaimsIdentity;
            //IList<Claim> claim = identity.Claims.ToList();
            //int userId = Convert.ToInt32(claim[2].Value);
            if (IsCorrectUser(resturantMenus[0].CustomerId))
            {
                return Unauthorized("DENIED");
            }

            var menu = await _context.Menu.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            _context.Menu.Remove(menu);
            await _context.SaveChangesAsync();

            return menu;
        }

        private bool MenuExists(int id)
        {
            return _context.Menu.Any(e => e.Id == id);
        }

        private bool IsCorrectUser(int customerId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            int userId = Convert.ToInt32(claim[2].Value);
            bool isAdmin = claim[3].Value == "1" ? true : false;
            if (isAdmin | userId == customerId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
