using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YooPoon.Common.WC.Entities
{
    /// <summary>
    /// 微信关注用户信息
    /// </summary>
    public class UserInfo : ErrorInfo
    {
        /// <summary>
        /// 1:关注;0:没有关注,获取不到其他信息
        /// </summary>
        public int subscribe { get; set; }

        public string openid { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname { get; set; }

        /// <summary>
        /// 1:男;2:女;0:未知
        /// </summary>
        public int? sex { get; set; }

        public string city { get; set; }

        public string country { get; set; }

        public string province { get; set; }

        public string language { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string headimgurl { get; set; }

        /// <summary>
        /// 最后一次关注的时间
        /// </summary>
        public int? subscribe_time { get; set; }

        public string unionid { get; set; }

        /// <summary>
        /// 公众号运营者对粉丝的备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 用户所在的分组ID
        /// </summary>
        public int? groupid { get; set; }
    }

    public class UserInfoList : ErrorInfo
    {
        public List<UserInfo> user_info_list { get; set; }
    }
}
