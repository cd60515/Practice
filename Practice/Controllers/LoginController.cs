using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Practice.Models;
using Practice.Service;
using Microsoft.EntityFrameworkCore;

namespace Practice.Controllers
{
    public class LoginController : Controller
    {
        private readonly MyContext _context;

        public LoginController(MyContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(USERS user)
        {
            //如果驗證成功，跳轉到主畫面
            if (ModelState.IsValid && LoginCheck(user))
            {
                RedirectToAction("USER");
                return View("USER/USER");
            }
            else //若失敗
            { //ModelState.AddModelError("PWD", "密碼驗證失敗");
                return View(user);
            }
            //return View(user);
        }

        //User身分驗證
        public bool LoginCheck(USERS user)
        {
            var login_user = _context.USERS.FirstOrDefaultAsync(x => x.ID == user.ID & x.PWD == user.PWD);
            if (login_user != null)
            { return true; }
            else
            { return false; }
        }
    }
}