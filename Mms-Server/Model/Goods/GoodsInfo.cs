using System;
using System.Collections.Generic;
using System.Text;

namespace Mms_Server.Model.Goods
{
    /// <summary>
    /// 商品信息
    /// </summary>
    public class GoodsInfo
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Spec { get; set; }

        public decimal RetailPrice { get; set; }

        public decimal PurchasePrice { get; set; }

        public int StorageNum { get; set; }

        public string SupplierID { get; set; }

        public string Creater { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Updater { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string SupplierName { get; set; }
    }
}
