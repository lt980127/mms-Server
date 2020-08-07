using System;
using System.Collections.Generic;
using System.Text;

namespace Mms_Server.Model.Member
{
    /// <summary>
    /// 会员信息
    /// </summary>
    public class MemberInfo
    {
        public int ID { get; set; }

        /// <summary>
        /// 会员卡号
        /// </summary>
        public string CardNum { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public string Integral { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        public string Creater { get; set; }

        public DateTime CreateTime { get; set; }

        public string Updater { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
