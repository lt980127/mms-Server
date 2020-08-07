using System;
using System.Collections.Generic;
using System.Text;

namespace Mms_Server.Common
{
    public class VMResult<T>
    {
        public VMResult()
        {
            if (typeof(T).IsClass && typeof(T) != typeof(string))
            {
                Data = System.Activator.CreateInstance<T>();
            }
            ResultCode = 1;
        }
        /// <summary>
        /// 0成功，非0错误
        /// </summary>
        public int ResultCode { get; set; }
        /// <summary>
        /// 文字描述信息
        /// </summary>
        public string ResultMsg { get; set; }
        /// <summary>
        /// 泛型自定义数据
        /// </summary>
        public T Data { get; set; }

    }
}
