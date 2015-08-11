using System.Collections.Generic;
using YooPoon.Core.Autofac;

namespace YooPoon.Common.WC.Common
{
    public interface IWCHelper:ISingletonDependency
    {
        string SendGet(string url, Dictionary<string, string> paramsDic);
        string SendPost(string url, string postData);

        string ConvertToXml(SortedDictionary<string, string> dic);

        string ConvertToJson(SortedDictionary<string, string> dic);


        string GenerateNonceStr();


        string GenerateTimeStamp();
    }
}