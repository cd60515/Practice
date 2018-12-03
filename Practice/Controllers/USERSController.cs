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

        //建構子，建立DB連線
        public USERSController(MyContext context)
        {
            _context = context;
        }

        //USER首頁
        public async Task<IActionResult> Index()
        {
            return View(await _context.USERS.ToListAsync());
        }

        //查詢USER
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

        //回傳建立USER頁面
        public IActionResult Create()
        {
            return View();
        }

        //建立USER的動作
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

        //回傳編輯User頁面
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

        //編輯User的動作
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

        //回傳刪除User的頁面
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

        //刪除User的動作
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var uSERS = await _context.USERS.FindAsync(id);
            _context.USERS.Remove(uSERS);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //判斷User是否已存在
        private bool USERSExists(string id)
        {
            return _context.USERS.Any(e => e.ID == id);
        }
    }
}
