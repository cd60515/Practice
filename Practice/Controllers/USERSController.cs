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
    public class USERSController : Controller
    {
        private readonly MyContext _context;

        public USERSController(MyContext context)
        {
            _context = context;
        }

        // GET: USERS
        public async Task<IActionResult> Index()
        {
            return View(await _context.USERS.ToListAsync());
        }

        // GET: USERS/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uSERS = await _context.USERS
                .FirstOrDefaultAsync(m => m.ID == id);
            if (uSERS == null)
            {
                return NotFound();
            }

            return View(uSERS);
        }

        // GET: USERS/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: USERS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,NAME,PWD")] USERS uSERS)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uSERS);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uSERS);
        }

        // GET: USERS/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uSERS = await _context.USERS.FindAsync(id);
            if (uSERS == null)
            {
                return NotFound();
            }
            return View(uSERS);
        }

        // POST: USERS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] //用來防止其他的網站可能偽造request，所以在對應的View上必須要有Html.AntiForgeryToken()
        public async Task<IActionResult> Edit(string id, [Bind("ID,NAME,PWD")] USERS uSERS)
        {
            if (id != uSERS.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uSERS);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!USERSExists(uSERS.ID))
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
            return View(uSERS);
        }

        // GET: USERS/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uSERS = await _context.USERS
                .FirstOrDefaultAsync(m => m.ID == id);
            if (uSERS == null)
            {
                return NotFound();
            }

            return View(uSERS);
        }

        // POST: USERS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var uSERS = await _context.USERS.FindAsync(id);
            _context.USERS.Remove(uSERS);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool USERSExists(string id)
        {
            return _context.USERS.Any(e => e.ID == id);
        }
    }
}
