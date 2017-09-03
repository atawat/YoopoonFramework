using System.Collections.Generic;
using YooPoon.Core.Autofac;

namespace YooPoon.Common.WC.Common
{
    public interface IWCCommonService:ISingletonDependency
    {
        string AccessToken { get; }

        string Token { get; }

        string JsAPITicket { get; }
        string AppId { get; }

        string AppSecret { get; }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        void RefreshToken();

        string MakeSign(SortedDictionary<string, string> dic);

        OAuthAccessToken GetOAuthAccessToken(string code);

        bool CheckSignature(string signature, string timestamp, string nonce, string token = null);
    }
}