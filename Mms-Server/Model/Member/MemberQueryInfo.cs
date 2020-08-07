using System;
using System.Collections.Generic;
using System.Text;

namespace Mms_Server.Model.Member
{
    /// <summary>
    /// 会员查询条件
    /// </summary>
    public class MemberQueryInfo
    {
        public string UserName { get; set; }

        public string CardNumber { get; set; }

        public int PayTypeID { get; set; }

        public DateTime? Birthday { get; set; }
    }
}
