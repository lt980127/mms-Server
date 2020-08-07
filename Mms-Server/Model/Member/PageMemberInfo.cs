using System;
using System.Collections.Generic;
using System.Text;

namespace Mms_Server.Model.Member
{
    public class PageMemberInfo 
    {
        public int total { get; set; }

        public IEnumerable<MemberInfo> MemberInfos { get; set; }
    }
}
