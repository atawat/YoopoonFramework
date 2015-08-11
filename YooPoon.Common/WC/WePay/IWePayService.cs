using System.Collections.Generic;
using System.Collections.Specialized;
using YooPoon.Core.Autofac;

namespace YooPoon.Common.WC.WePay
{
    public interface IWePayService:IDependency
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        string Mchid { get; }

        /// <summary>
        /// 商户支付密钥
        /// </summary>
        string Key { get; }

        string ProductId { get; }

        object UnifiedOrder(SortedDictionary<string, string> dic);

        string MakeSign(SortedDictionary<string,string> dic);
    }
}