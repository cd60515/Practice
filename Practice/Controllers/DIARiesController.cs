using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practice.Models;
using Practice.Service;

namespace Practice.Controllers
{
    public class DIARiesController : Controller
    {
        private readonly MyContext _context;

        public DIARiesController(MyContext context)
        {
            _context = context;
        }

        // GET: DIARies
        public async Task<IActionResult> Index()
        {
            return View(await _context.DIARY.ToListAsync());
        }

        // GET: DIARies/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dIARY = await _context.DIARY
                .FirstOrDefaultAsync(m => m.USER_ID == id);
            if (dIARY == null)
            {
                return NotFound();
            }

            return View(dIARY);
        }

        // GET: DIARies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DIARies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("USER_ID,DIARY_ID,DIARY_TITLE,DIARY_DATE,DIARY_TEXT,WEATHER")] DIARY dIARY)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dIARY);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dIARY);
        }

        // GET: DIARies/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dIARY = await _context.DIARY.FindAsync(id);
            if (dIARY == null)
            {
                return NotFound();
            }
            return View(dIARY);
        }

        // POST: DIARies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("USER_ID,DIARY_ID,DIARY_TITLE,DIARY_DATE,DIARY_TEXT,WEATHER")] DIARY dIARY)
        {
            if (id != dIARY.USER_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dIARY);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DIARYExists(dIARY.USER_ID))
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
            return View(dIARY);
        }

        // GET: DIARies/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dIARY = await _context.DIARY
                .FirstOrDefaultAsync(m => m.USER_ID == id);
            if (dIARY == null)
            {
                return NotFound();
            }

            return View(dIARY);
        }

        // POST: DIARies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var dIARY = await _context.DIARY.FindAsync(id);
            _context.DIARY.Remove(dIARY);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DIARYExists(string id)
        {
            return _context.DIARY.Any(e => e.USER_ID == id);
        }
    }
}
