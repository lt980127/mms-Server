using System;
using System.Collections.Generic;
using System.Text;

namespace Mms_Server.Model.Supplier
{
    public class PageSupplierInfo
    {
        public int total { get; set; }

        public IEnumerable<SupplierInfo> SupplierInfos { get; set; }
    }
}
