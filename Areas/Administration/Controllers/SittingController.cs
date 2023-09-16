using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using Group_BeanBooking.Areas.Administration.Controllers;
using Group_BeanBooking.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;


namespace Group_BeanBooking.Areas.Administration.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class SittingController : AdministrationAreaController
    {


        public SittingController(ApplicationDbContext context) : base(context)
        {


        }

        // GET: Administration/Sitting
        public async Task<IActionResult> Index()
        {
            var sittings = await _context.Sittings.Include(s => s.Restaurant).Include(s => s.Type).ToListAsync();
            return View(sittings);
        }

        // GET: Administration/Sitting/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sittings == null)
            {
                return NotFound();
            }

            var sitting = await _context.Sittings
                .Include(s => s.Restaurant)
                .Include(s => s.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sitting == null)
            {
                return NotFound();
            }

            return View(sitting);
        }

        // GET: Administration/Sitting/Create
        public IActionResult Create()
        {

            var start = DateTime.Now.Date.AddDays(1).AddHours(7);
            var m = new Models.Sitting.Create
            {
                SittingTypes = new SelectList(_context.SittingTypes, "Id", "Name"),
                Start = start,
                End = start.AddHours(4)
            };

            return View(m);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Sitting.Create m)
        {
            if (ModelState.IsValid)
            {
                var sitting = new Sitting
                {
                    Name = m.Name,
                    Start = m.Start,
                    End = m.End,
                    Capacity = m.Capacity,
                    Closed = m.Closed,
                    TypeId = m.TypeId,
                    RestaurantId = 1
                };
                //= _mapper.Map<Data.Sitting>(m);
                _context.Sittings.Add(sitting);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            m.SittingTypes = new SelectList(_context.SittingTypes, "Id", "Name", m.TypeId);
            return View(m);
        }

        // GET: Administration/Sitting/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sittings == null)
            {
                return NotFound();
            }

            var sitting = await _context.Sittings.FindAsync(id);
            if (sitting == null)
            {
                return NotFound();
            }

            var m = new Models.Sitting.Edit
            {
                Name = sitting.Name,
                Start = sitting.Start,
                End = sitting.End,
                Capacity = sitting.Capacity,
                Closed = sitting.Closed,
                TypeId = sitting.TypeId,
            };
     
            //_mapper.Map<Models.Sitting.Edit>(sitting);

           m.SittingTypes = new SelectList(_context.SittingTypes, "Id", "Name", m.TypeId);


            return View(m);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Models.Sitting.Edit m)
        {
            var sitting = _context.Sittings.FirstOrDefault(s => s.Id == m.Id);

            if (sitting == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {   sitting.Start = m.Start;
                    sitting.End = m.End;
                    sitting.Capacity = m.Capacity;
                    sitting.Closed = m.Closed;
                    sitting.Name = m.Name;
                    sitting.TypeId = m.TypeId;
                    //_mapper.Map(m, sitting);
                  
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SittingExists(sitting.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            m.SittingTypes = new SelectList(_context.SittingTypes, "Id", "Name", m.TypeId);
            return View(m);
        }

        //GET: Administration/Sitting/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sittings == null)
            {
                return NotFound();
            }

            var sitting = await _context.Sittings
                .Include(s => s.Restaurant)
                .Include(s => s.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sitting == null)
            {
                return NotFound();
            }

            return View(sitting);
        }

        // POST: Administration/Sitting/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sittings == null)
            {
                return Problem("Entity set 'ApplicationDb_context.Sittings'  is null.");
            }
            var sitting = await _context.Sittings.FindAsync(id);
            if (sitting != null)
            {
                _context.Sittings.Remove(sitting);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SittingExists(int id)
        {
            return (_context.Sittings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
