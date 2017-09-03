using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using YooPoon.Common.WC.XmlHelper;

namespace YooPoon.Common.WC.XmlEntities
{
    [XmlType("xml")]
    public class ReceiveTextModel
    {
        /// <summary>
        /// 接收方帐号（收到的OpenID）
        /// </summary>
        public MyCDATA ToUserName { get; set; }

        /// <summary>
        /// 开发者微信号
        /// </summary>
        public MyCDATA FromUserName { get; set; }

        public MyCDATA CreateTime { get; set; }

        /// <summary>
        /// text
        /// </summary>
        public MyCDATA MsgType { get; set; }

        public MyCDATA Content { get; set; }

        public MyCDATA MsgId { get; set; }
    }
}
