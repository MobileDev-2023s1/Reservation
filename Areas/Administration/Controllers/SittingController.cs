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
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using ZstdSharp.Unsafe;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                End = start.AddHours(4),

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

                if (m.Repeats == 0)
                {
                    _context.Sittings.Add(sitting);
                    await _context.SaveChangesAsync();
                }

                else
                {

                    sitting.Guid = Guid.NewGuid();
                    var sittings = new List<Sitting> { sitting };
                    DateTime additionalStart = new();
                    DateTime additionalEnd = new();
                    bool[] RepeatPattern =
                        {m.Sunday,m.Monday,m.Tuesday,m.Wednesday,m.Thursday,m.Friday,m.Saturday};
                    double days = 0;


                    for (int j = 0; j < 7; j++)
                    {
                        if (RepeatPattern[j] == true)
                        {
                            for (int i = 1; i <= m.Repeats; i++)
                            {
                                if (j == (int)m.Start.DayOfWeek)
                                {
                                    days = i * 7 * m.Interval;
                                    additionalStart = m.Start.AddDays(days);
                                    additionalEnd = m.End.AddDays(days);
                                }
                                else
                                {
                                    if (j> (int)m.Start.DayOfWeek)
                                    {
                                        days = ((j - (int)m.Start.DayOfWeek) % 7);

                                    }
                                    else
                                    {
                                        days = ((j - (int)m.Start.DayOfWeek) % 7) + (i * 7 * m.Interval);
                                    }
                                    
                                    additionalStart = m.Start.AddDays(days);
                                    additionalEnd = m.End.AddDays(days);
                                }
                                var additionalSitting = new Sitting
                                {
                                    Guid = sitting.Guid,
                                    Name = m.Name,
                                    Start = additionalStart,
                                    End = additionalEnd,
                                    Capacity = m.Capacity,
                                    Closed = m.Closed,
                                    TypeId = m.TypeId,
                                    RestaurantId = 1
                                };
                                sittings.Add(additionalSitting);

                            }
                        }
                    }
                    _context.Sittings.AddRange(sittings);
                    await _context.SaveChangesAsync();
                }
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

            m.SittingTypes = new SelectList(_context.SittingTypes, "Id", "Name", m.TypeId);
            ViewData["guid"] = sitting.Guid;
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
                {
              
                    if (sitting.Guid == null)
                    {
                      
                        sitting.Start = m.Start;
                        sitting.End = m.End;
                        sitting.Capacity = m.Capacity;
                        sitting.Closed = m.Closed;
                        sitting.Name = m.Name;
                  
                    }
                    else
                    {
                        var sittings = _context.Sittings.Where(s => s.Guid == sitting.Guid).OrderBy(s => s.Start).ToList();
                        var sittingIndex = 0;
                        var overLap = false;
                        for (int i = 0; i < sittings.Count-1; i++)
                        {
                            if (sitting.Id == sittings[i].Id){sittingIndex = i;}
                        } 
                        if (m.Range == "This")
                        {                           
                            for (int i = 0; i < sittings.Count-1; i++)
                            {
                                if (sitting.Id != sittings[i].Id)
                                {
                                    if (sittings[i].Start < m.End && m.Start < sittings[i].End)
                                    { overLap = true; }
                                }
                            }
                            if (!overLap)
                            {
                                sitting.Start = m.Start;
                                sitting.End = m.End;
                                sitting.Capacity = m.Capacity;
                                sitting.Closed = m.Closed;
                                sitting.Name = m.Name;
                            }
                        }

                        else
                        {
                            var sittingsToRemove = sittings;
                            if (m.Range == "ThisAndAfter")
                            {
                                sittingsToRemove.RemoveRange(0, sittingIndex);
                            }                             
                            _context.Sittings.RemoveRange(sittingsToRemove);                         
                        }                   
                    }                   
                await _context.SaveChangesAsync();}
                catch (DbUpdateConcurrencyException)
                    {
                    if (!SittingExists(sitting.Id)){return NotFound();}
                    else{throw;}
                    }
                await Create(m);
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

