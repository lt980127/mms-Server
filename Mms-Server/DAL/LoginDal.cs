using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Mms_Server.Common;
using Mms_Server.Model;

namespace Mms_Server.DAL
{
    public class LoginDal
    {
        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }

        public async Task<VMResult<UserInfo>> Login(string account, string password)
        {
            VMResult<UserInfo> r = new VMResult<UserInfo>();
            string md5PassWord = EncryptString(password);
            using (var conn=DapperHelper.CreateConnection())
            {
                string strSql = @"SELECT * FROM [User] u WHERE u.Account=@Account and u.Password=@Password";
                var value = await conn.QueryFirstOrDefaultAsync<UserInfo>(strSql, new
                {
                    Account = account,
                    Password = md5PassWord
                });
                if (value == null) 
                {
                    r.ResultMsg = "账号或用户名错误";
                }
                else
                {
                    r.ResultCode = 0;
                    r.Data = value;
                }
                
            }

            return r;
        }
    }
}
