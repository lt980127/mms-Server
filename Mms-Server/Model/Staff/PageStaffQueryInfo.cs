using System;
using System.Collections.Generic;
using System.Text;

namespace Mms_Server.Model.Staff
{
    public class PageStaffQueryInfo
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int PageSize { get; set; }

        public StaffQueryInfo StaffQueryInfo { get; set; }
    }
}
