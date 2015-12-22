using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using YooPoon.Common.WC.Common;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;

namespace YooPoon.Common.WC.WePay
{
    public class WePayService:IWePayService
    {
        private readonly DataSettings _dataSettings;
        private readonly ILog _log;
        private readonly IWCHelper _helper;
        private readonly IWCCommonService _commonService;

        public WePayService(ILog log, IWCHelper helper,IWCCommonService commonService ,DataSettings dataSettings)
        {
            _log = log;
            _helper= helper;
            _dataSettings = dataSettings;
            _commonService = commonService;
        }
        public string Mchid
        {
            get
            {
                return _dataSettings.RawDataSettings["Mchid"];
            }
        }

        public string Key
        {
            get
            {
                return _dataSettings.RawDataSettings["Key"]; 
            }
        }

        public string ProductId
        {
            get
            {
                return _dataSettings.RawDataSettings["ProductId"];
            }
        }

        public object UnifiedOrder(SortedDictionary<string,string> dic)
        {
            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";

            //检测必填参数
            if (!dic.ContainsKey("out_trade_no"))
            {
                throw new Exception("缺少统一支付接口必填参数out_trade_no！");
            }
            if (!dic.ContainsKey("body"))
            {
                throw new Exception("缺少统一支付接口必填参数body！");
            }
            if (!dic.ContainsKey("total_fee"))
            {
                throw new Exception("缺少统一支付接口必填参数total_fee！");
            }
            if (!dic.ContainsKey("trade_type"))
            {
                throw new Exception("缺少统一支付接口必填参数trade_type！");
            }

            //关联参数
            if (dic["trade_type"] == "JSAPI" && !dic.ContainsKey("openid"))
            {
                throw new Exception("统一支付接口中，缺少必填参数openid！trade_type为JSAPI时，openid为必填参数！");
            }
            if (dic["trade_type"] == "NATIVE" && !dic.ContainsKey("product_id"))
            {
                throw new Exception("统一支付接口中，缺少必填参数product_id！trade_type为JSAPI时，product_id为必填参数！");
            }
            if (!dic.ContainsKey("notify_url"))
            {
                throw new Exception("异步通知Url未设置");
            }

            //添加公共部分参数
            dic.Add("appid",_commonService.AppId);
            dic.Add("mch_id",Mchid);
            dic.Add("spbill_create_ip","127.0.0.1");
            dic.Add("nonce_str",_helper.GenerateNonceStr());

            dic.Add("sign", MakeSign(dic));
            var xml = _helper.ConvertToXml(dic);
            var response = _helper.SendPost(url, xml);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(response);
            return xmlDoc;
        }

        public string MakeSign(SortedDictionary<string,string> dic)
        {
            var urlFormatString = string.Join("&", dic.Select(d => d.Key + "=" + d.Value));
            urlFormatString += "&key=" + Key; //拼接商户Key
            //MD5加密
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(urlFormatString));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToUpper();
        }
    }
}