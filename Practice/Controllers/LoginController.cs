﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Practice.Models;
using Practice.Service;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Web;
using System.Collections;

namespace Practice.Controllers
{
    public class LoginController : Controller
    {
        private readonly MyContext _context;

        //建構子，建立DB連線
        public LoginController(MyContext context)
        {
            _context = context;
        }

        //回傳Login頁面
        public IActionResult Login()
        {
            return View();
        }

        //登入的動作
        [HttpPost]
        public IActionResult Login(USERS user)
        {
            Hashtable rtn = null; //先宣告一個空的hash table等一下儲存結果用

            //如果驗證成功，跳轉到主畫面
            if (ModelState.IsValid && LoginCheck(user))
            {
                //RedirectToAction("USER");
                //return View("USER/USER"); //本來想要透過.Net自己做跳轉頁面，但後續決定在前端處理
                rtn = new Hashtable()
                {
                    { "userid", user.ID },
                    { "result", true }
                };
            }
            else //若失敗
            {
                //ModelState.AddModelError("PWD", "密碼驗證失敗");
                //return View(user);
                rtn = new Hashtable()
                {
                    { "userid", user.ID },
                    { "result", false }
                };
            }

            //return Json(user);
            return Content(JsonConvert.SerializeObject(new {result = rtn}), "application/json");
        }

        //User身分驗證
        public bool LoginCheck(USERS user)
        {
            //確認是否有該User存在
            var login_user = _context.USERS.FirstOrDefaultAsync(x => x.ID == user.ID & x.PWD == user.PWD).Result;
            if (login_user != null)
            { return true; }
            else
            { return false; }
        }
    }
}