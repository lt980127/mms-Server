using System;
using System.Collections.Generic;
using System.Text;

namespace Mms_Server.Model.Goods
{
    public class pageGoodsInfo
    {
        public int total { get; set; }

        public IEnumerable<GoodsInfo> goodsInfos { get; set; }
    }
}
