using System;
using System.Collections.Generic;
using System.Text;

namespace Mms_Server.Model
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        public int ID { get; set; }

        public string UserName { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public string Creater { get; set; }

        public DateTime CreateTime { get; set; }

        public string Updater { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
