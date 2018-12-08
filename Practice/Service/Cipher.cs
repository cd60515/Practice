using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Service
{
    public class Cipher
    {
        /// <summary>   
        /// 取得 MD5 編碼後的 Hex 字串   
        /// 加密後為 32 Bytes Hex String (16 Byte)   
        /// </summary>   
        /// <span  name="original" class="mceItemParam"></span>原始字串</param>   
        /// <returns></returns>   
        public string GetMD5(string original)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] b = md5.ComputeHash(Encoding.UTF8.GetBytes(original));
            return BitConverter.ToString(b).Replace("-", string.Empty);
        }
    }
}
