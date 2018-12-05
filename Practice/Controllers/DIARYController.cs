using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Practice.Models;
using Practice.Service;

namespace Practice.Controllers
{
    public class DIARYController : Controller
    {
        private readonly MyContext _context;

        //建構子，建立DB連線
        public DIARYController(MyContext context)
        {
            _context = context;

        }
        public IActionResult DIARY_Index()
        {
            return View();
        }

        //抓取日記資料
        [HttpPost]
        public IActionResult Get_DIARY(string SEARCH_DATE)
        {
            var DIARIES = _context.DIARY.ToList<DIARY>();
            var USERS = _context.USERS.ToList<USERS>();
            List<DIARY> rtn = new List<DIARY>(); //先宣告一個空的hash table等一下儲存結果用
            rtn.Add(new DIARY { DIARY_TITLE="TITLE1", DIARY_DATE=DateTime.Today.ToString("yyyy/MM/dd"), DIARY_TEXT="TEXT1", USER_ID="POAN", DIARY_ID="1", WEATHER="Sun"});
            rtn.Add(new DIARY { DIARY_TITLE = "TITLE2", DIARY_DATE = DateTime.Today.ToString("yyyy/MM/dd"), DIARY_TEXT = "TEXT2", USER_ID = "POAN", DIARY_ID = "2", WEATHER = "Sun" });
            return Content(JsonConvert.SerializeObject(new { rtn = DIARIES }), "application/json");
        }

        //建立日記
        [HttpPost]
        public async Task<IActionResult> Insert_DIARY(DIARY diary)
        {
            //diary = new DIARY { DIARY_TITLE = "TITLE3", DIARY_DATE = DateTime.Today.ToString("yyyy/MM/dd"), DIARY_TEXT = "TEXT3", USER_ID = "POAN", WEATHER = "Sun" };
            await _context.DIARY.AddAsync(diary);
            await _context.SaveChangesAsync();
            return Content(JsonConvert.SerializeObject(new { rtn = new { result=true} }), "application/json");
        }

        //修改日記
        [HttpPost]
        public async Task<IActionResult> Modify_DIARY(DIARY diary)
        {
            //diary = new DIARY { DIARY_TITLE = "TITLE3", DIARY_DATE = DateTime.Today.ToString("yyyy/MM/dd"), DIARY_TEXT = "TEXT3", USER_ID = "POAN", WEATHER = "Sun" }; //測試用
            _context.DIARY.Update(diary);
            await _context.SaveChangesAsync();
            return Content(JsonConvert.SerializeObject(new { rtn = new { result = true } }), "application/json");
        }

        //刪除日記
        [HttpPost]
        public async Task<IActionResult> Delete_DIARY(string DIARY_ID,string USER_ID)
        {
            var dIARY = await _context.DIARY.FindAsync(USER_ID,DIARY_ID);
            _context.DIARY.Remove(dIARY);
            await _context.SaveChangesAsync();
            return Content(JsonConvert.SerializeObject(new { rtn = new { result = true } }), "application/json");
        }
    }
}