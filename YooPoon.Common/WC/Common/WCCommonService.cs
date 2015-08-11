using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;

namespace YooPoon.Common.WC.Common
{
    public class WCCommonService : IWCCommonService
    {
        private readonly ILog _log;
        private readonly IWCHelper _helper;
        private readonly DataSettings _dataSettings;

        private string _accessToken;
        private int _tokenExpiresIn;
        private DateTime _tokenUpdTime;
        private bool _tokenRefreshLock = false;

        private string _jsAPITicket;
        private int _ticketExpiresIn;
        private DateTime _ticketUpdTime;
        private bool _ticketRefreshLock = false;

        public WCCommonService(ILog log, IWCHelper helper, DataSettings dataSettings)
        {
            _log = log;
            _helper = helper;
            _dataSettings = dataSettings;
        }

        public string AccessToken
        {
            get
            {
                if (!string.IsNullOrEmpty(_accessToken) && _tokenUpdTime != DateTime.MinValue && _tokenUpdTime.AddSeconds(_tokenExpiresIn) < DateTime.Now)
                    return _accessToken;
                if (!_tokenRefreshLock)
                    RefreshToken();
                return _accessToken;
            }
        }

        public string JsAPITicket
        {
            get
            {
                if (!string.IsNullOrEmpty(_jsAPITicket) && _ticketUpdTime != DateTime.MinValue && _ticketUpdTime.AddSeconds(_ticketExpiresIn) > DateTime.Now)
                    return _jsAPITicket;
                if (!_ticketRefreshLock)
                    RefreshTicket();
                return _jsAPITicket;
            }
        }

        public string AppId
        {
            get
            {
                return _dataSettings.RawDataSettings["AppId"];
            }
        }

        public string AppSecret
        {
            get
            {
                return _dataSettings.RawDataSettings["AppSecret"];
            }
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        public void RefreshToken()
        {
            _tokenRefreshLock = true;
            var param = new Dictionary<string, string>
            {
                {"grant_type", "client_credential"},
                {"appid", AppId},
                {"secret", AppSecret}
            };
            var reponseStr = _helper.SendGet("https://api.weixin.qq.com/cgi-bin/token", param);
            if (reponseStr == null)
            {
                _log.Error("获取AccessToken出错，请检查错误");
                return;
            }
            var responseObj = new { access_token = "", expires_in = 0 };
            var responseJson = JsonConvert.DeserializeAnonymousType(reponseStr, responseObj);
            if (responseJson != null)
            {
                _accessToken = responseJson.access_token;
                _tokenExpiresIn = responseJson.expires_in;
                _tokenUpdTime = DateTime.Now;
            }
            else
            {
                var responseErrorObj = new { errcode = "", errmsg = "" };
                var errorJson = JsonConvert.DeserializeAnonymousType(reponseStr, responseErrorObj);
                _log.Error("获取AccessToken出错，错误代码{0}，错误信息：{1}", errorJson.errcode, errorJson.errmsg);
            }
            _tokenRefreshLock = false;
        }

        private void RefreshTicket()
        {
            _ticketRefreshLock = true;
            var param = new Dictionary<string, string>
            {
                {"access_token", AccessToken},
                {"type", "jsapi"}
            };
            var reponseStr = _helper.SendGet("https://api.weixin.qq.com/cgi-bin/ticket/getticket", param);
            if (reponseStr == null)
            {
                _log.Error("获取AccessToken出错，请检查错误");
                return;
            }
            //Response容器
            var responseObj = new { errcode = 0, errmsg = "", ticket = "", expires_in = 0 };
            var responseJson = JsonConvert.DeserializeAnonymousType(reponseStr, responseObj);
            if (responseJson.errcode == 0)
            {
                _jsAPITicket = responseJson.ticket;
                _ticketExpiresIn = responseJson.expires_in;
                _ticketUpdTime = DateTime.Now;
            }
            else
            {
                _log.Error("获取AccessToken出错，错误代码{0}，错误信息：{1}", responseJson.errcode, responseJson.errmsg);
            }
            _ticketRefreshLock = false;
        }

        public string MakeSign(SortedDictionary<string, string> dic)
        {
            var urlFormatString = string.Join("&", dic.Select(d => d.Key + "=" + d.Value));
            //MD5加密
            var sha = SHA1.Create();
            var bs = sha.ComputeHash(Encoding.UTF8.GetBytes(urlFormatString));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToUpper();
        }

        public OAuthAccessToken GetOAuthAccessToken(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;
            var param = new Dictionary<string, string>()
            {
                {"appid",AppId},
                {"secret",AppSecret},
                {"code",code},
                {"grant_type","authorization_code"}
            };
            var response = _helper.SendGet("https://api.weixin.qq.com/sns/oauth2/access_token", param);
            return JsonConvert.DeserializeObject<OAuthAccessToken>(response);
        }
    }
}