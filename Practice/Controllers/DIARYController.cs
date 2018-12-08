using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using Practice.Models;
using Practice.Service;

namespace Practice.Controllers
{
    public class DIARYController : Controller
    {
        private readonly MyContext _context;
        private readonly IConfiguration _config;

        //建構子，建立DB連線
        public DIARYController(MyContext context, IConfiguration config)
        {
            _context = context;
            _config = config;

        }

        //回傳日記頁面
        public IActionResult DIARY_Index()
        {
            return View();
        }

        //抓取日記資料
        [HttpPost]
        public IActionResult Get_DIARY(string SEARCH_DATE, string USER_ID)
        {
            var DIARIES = _context.DIARY.ToList<DIARY>().Where(x=>x.USER_ID==USER_ID).OrderByDescending(x=>x.DIARY_DATE);
            var USERS = _context.USERS.ToList<USERS>();
            List<DIARY> rtn = new List<DIARY>(); //先宣告一個空的hash table等一下儲存結果用
            //rtn.Add(new DIARY { DIARY_TITLE="TITLE1", DIARY_DATE=DateTime.Today.ToString("yyyy/MM/dd"), DIARY_TEXT="TEXT1", USER_ID="POAN", DIARY_ID="1", WEATHER="Sun"});
            //rtn.Add(new DIARY { DIARY_TITLE = "TITLE2", DIARY_DATE = DateTime.Today.ToString("yyyy/MM/dd"), DIARY_TEXT = "TEXT2", USER_ID = "POAN", DIARY_ID = "2", WEATHER = "Sun" });
            return Content(JsonConvert.SerializeObject(new { rtn = DIARIES }), "application/json");
        }

        //抓取日記資料 (用procedure的方式)
        [HttpPost]
        public IActionResult Get_DIARY2(string SEARCH_DATE, string USER_ID)
        {
            
            DataTable dt = new DataTable(); //創建一個空的DataTable等一下用來裝資料
            
            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DefaultConnection"))) //建立sql連線字串物件
            {
                string procedure_name = "SelectAllDiaries"; 

                using (SqlCommand sqlCmd = new SqlCommand(procedure_name, sqlConn)) //建立sqlcommand物件
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@USER_ID", USER_ID);
                    sqlConn.Open(); //建立DB連線
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                    {
                        sqlAdapter.Fill(dt);
                    }
                }
            }
            return Content(JsonConvert.SerializeObject(new { rtn = dt }), "application/json");
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

        //建立日記 (用procedure的方式)
        [HttpPost]
        public async Task<IActionResult> Insert_DIARY2(DIARY diary)
        {
            DataTable dt = new DataTable(); //創建一個空的DataTable等一下用來裝資料

            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DefaultConnection"))) //建立sql連線字串物件
            {
                string procedure_name = "InsertDiary";

                using (SqlCommand sqlCmd = new SqlCommand(procedure_name, sqlConn)) //建立sqlcommand物件
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@USER_ID", diary.USER_ID);
                    sqlCmd.Parameters.AddWithValue("@DIARY_ID", Guid.NewGuid().ToString());
                    sqlCmd.Parameters.AddWithValue("@DIARY_TITLE", diary.DIARY_TITLE);
                    sqlCmd.Parameters.AddWithValue("@DIARY_TEXT", diary.DIARY_TEXT);
                    sqlCmd.Parameters.AddWithValue("@DIARY_DATE", diary.DIARY_DATE);
                    sqlCmd.Parameters.AddWithValue("@WEATHER", diary.WEATHER);
                    sqlConn.Open(); //建立DB連線
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                    {
                        try
                        {
                            sqlCmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("用procedure建立日記的時候出錯" + ex.Message);
                        }
                    }
                }
            }
            return Content(JsonConvert.SerializeObject(new { rtn = new { result = true } }), "application/json");
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

        //修改日記
        [HttpPost]
        public async Task<IActionResult> Modify_DIARY2(DIARY diary)
        {
            DataTable dt = new DataTable(); //創建一個空的DataTable等一下用來裝資料

            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DefaultConnection"))) //建立sql連線字串物件
            {
                string procedure_name = "UpdateDiary";

                using (SqlCommand sqlCmd = new SqlCommand(procedure_name, sqlConn)) //建立sqlcommand物件
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@USER_ID", diary.USER_ID);
                    sqlCmd.Parameters.AddWithValue("@DIARY_ID", diary.DIARY_ID);
                    sqlCmd.Parameters.AddWithValue("@DIARY_TITLE", diary.DIARY_TITLE);
                    sqlCmd.Parameters.AddWithValue("@DIARY_TEXT", diary.DIARY_TEXT);
                    sqlCmd.Parameters.AddWithValue("@DIARY_DATE", diary.DIARY_DATE);
                    sqlCmd.Parameters.AddWithValue("@WEATHER", diary.WEATHER);
                    sqlConn.Open(); //建立DB連線
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                    {
                        try
                        {
                            sqlCmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("用procedure修改日記的時候出錯" + ex.Message);
                        }
                    }
                }
            }
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

        //刪除日記 (用procedure的方式)
        [HttpPost]
        public async Task<IActionResult> Delete_DIARY2(string DIARY_ID, string USER_ID)
        {
            DataTable dt = new DataTable(); //創建一個空的DataTable等一下用來裝資料

            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DefaultConnection"))) //建立sql連線字串物件
            {
                string procedure_name = "DeleteDiary";

                using (SqlCommand sqlCmd = new SqlCommand(procedure_name, sqlConn)) //建立sqlcommand物件
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@USER_ID", USER_ID);
                    sqlCmd.Parameters.AddWithValue("@DIARY_ID", DIARY_ID);
                    sqlConn.Open(); //建立DB連線
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                    {
                        try
                        {
                            sqlCmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("用procedure刪除日記的時候出錯" + ex.Message);
                        }
                    }
                }
            }
            return Content(JsonConvert.SerializeObject(new { rtn = new { result = true } }), "application/json");
        }
    }
}