using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YooPoon.Common.WC.Entities
{
    /// <summary>
    /// 微信返回错误信息
    /// </summary>
    public class ErrorInfo
    {
        /// <summary>
        /// 错误代码，参考： https://mp.weixin.qq.com/wiki/10/6380dc743053a91c544ffd2b7c959166.html
        /// </summary>
        public int? errcode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }
    }
}
