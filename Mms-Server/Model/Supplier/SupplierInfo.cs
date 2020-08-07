using System;
using System.Collections.Generic;
using System.Text;

namespace Mms_Server.Model.Supplier
{
    /// <summary>
    /// 供应商信息
    /// </summary>
    public class SupplierInfo
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 公司法人
        /// </summary>
        public string LinkName { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public string Creater { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Updater { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}
