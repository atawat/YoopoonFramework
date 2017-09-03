using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using YooPoon.Common.WC.Entities;
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
        private readonly object _tokenRefreshLock = new object();

        private string _jsAPITicket;
        private int _ticketExpiresIn;
        private DateTime _ticketUpdTime;
        private readonly object _ticketRefreshLock = new object();

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
                if (!string.IsNullOrEmpty(_accessToken) && _tokenUpdTime != DateTime.MinValue && _tokenUpdTime.AddSeconds(_tokenExpiresIn) > DateTime.Now)
                    return _accessToken;
                lock (_tokenRefreshLock)
                {
                    if (!string.IsNullOrEmpty(_accessToken) && _tokenUpdTime != DateTime.MinValue && _tokenUpdTime.AddSeconds(_tokenExpiresIn) > DateTime.Now)
                        return _accessToken;
                    RefreshToken();
                }

                return _accessToken;
            }
        }

        /// <summary>
        /// 用户自己配置的消息服务器Token
        /// </summary>
        public string Token
        {
            get
            {
                return _dataSettings.RawDataSettings["Token"];
            }
        }

        public string JsAPITicket
        {
            get
            {
                if (!string.IsNullOrEmpty(_jsAPITicket) && _ticketUpdTime != DateTime.MinValue && _ticketUpdTime.AddSeconds(_ticketExpiresIn) > DateTime.Now)
                    return _jsAPITicket;
                lock (_ticketRefreshLock)
                {
                    if (!string.IsNullOrEmpty(_jsAPITicket) && _ticketUpdTime != DateTime.MinValue && _ticketUpdTime.AddSeconds(_ticketExpiresIn) > DateTime.Now)
                        return _jsAPITicket;
                    RefreshTicket();
                }

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
            if (responseJson.access_token != null)
            {
                _accessToken = responseJson.access_token;
                _tokenExpiresIn = responseJson.expires_in;
                _tokenUpdTime = DateTime.Now;
            }
            else
            {
                var errorJson = JsonConvert.DeserializeObject<MessageResultModel>(reponseStr);
                _log.Error("获取AccessToken出错，错误代码{0}，错误信息：{1}", errorJson.Errcode, errorJson.ErrMsg);
            }
        }

        private void RefreshTicket()
        {
            var param = new Dictionary<string, string>
            {
                {"access_token", AccessToken},
                {"type", "jsapi"}
            };
            var reponseStr = _helper.SendGet("https://api.weixin.qq.com/cgi-bin/ticket/getticket", param);
            if (reponseStr == null)
            {
                _log.Error("获取JsAPITicket出错，请检查错误");
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
                _log.Error("获取JsAPITicket出错，错误代码{0}，错误信息：{1}", responseJson.errcode, responseJson.errmsg);
            }
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

        #region 获取用户信息

        /// <summary>
        /// 网页授权OAuth获取用户信息（认证的服务号）
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public OAuthAccessToken GetOAuthAccessToken(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;
            var param = new Dictionary<string, string>()
            {
                {"appid", AppId},
                {"secret", AppSecret},
                {"code", code},
                {"grant_type", "authorization_code"}
            };
            var response = _helper.SendGet("https://api.weixin.qq.com/sns/oauth2/access_token", param);
            return JsonConvert.DeserializeObject<OAuthAccessToken>(response);
        }

        /// <summary>
        /// 获取已关注公众号的用户的个人信息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="lang"></param>
        public UserInfo GetUserInfo(string openId, string lang = "zh_CN")
        {
            if (string.IsNullOrEmpty(openId))
            {
                return null;
            }
            var param = new Dictionary<string, string>()
            {
                {"access_token", AccessToken},
                {"openid", openId },
                {"lang", lang }
            };

            var response = _helper.SendGet("https://api.weixin.qq.com/cgi-bin/user/info", param);
            var result = JsonConvert.DeserializeObject<UserInfo>(response);
            if (result.errcode != 0)
            {
                _log.Error("获取用户信息出错，错误代码{0}，错误信息：{1}", result.errcode, result.errmsg);
                return null;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 批量获取用户基本信息（最多支持一次拉取100条）
        /// </summary>
        /// <param name="openIds"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public UserInfoList GetUsersInfo(List<string> openIds, string lang = "zh-CN")
        {
            var users = openIds.Select(o => new { openid = o, lang = lang }).ToArray();
            var postData = new { user_list = users };
            var postDataJson = JsonConvert.SerializeObject(postData);

            var response = _helper.SendPost(string.Format("https://api.weixin.qq.com/cgi-bin/user/info/batchget?access_token={0}", AccessToken), postDataJson, null);
            var result = JsonConvert.DeserializeObject<UserInfoList>(response);
            if (result.errcode != 0)
            {
                _log.Error("批量获取用户基本信息出错，错误代码{0}，错误信息：{1}", result.errcode, result.errmsg);
                return null;
            }
            else
            {
                return result;
            }
        }

        #endregion

        #region 微信登录

        /// <summary>
        /// 检查签名(微信认证)
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool CheckSignature(string signature, string timestamp, string nonce, string token = null)
        {
            return signature == GetSignature(timestamp, nonce, token);
        }

        public string GetSignature(string timestamp, string nonce, string token = null)
        {
            token = token ?? Token;
            var arr = new[] { token, timestamp, nonce }.OrderBy(z => z).ToArray();
            var arrString = string.Join("", arr);
            var sha1 = SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            StringBuilder sb = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                sb.AppendFormat("{0:x2}", b);
            }

            return sb.ToString();
        }

        #endregion
    }
}