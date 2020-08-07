using System;
using System.Collections.Generic;
using System.Text;

namespace Mms_Server.Model.Staff
{
    public class PageStaffInfo
    {
        public int total { get; set; }

        public IEnumerable<StaffInfo> StaffInfos { get; set; }
    }
}
