using System.Collections.Generic;
using Newtonsoft.Json;
using YooPoon.Common.WC.Common;
using YooPoon.Common.WC.JsonParser;
using YooPoon.Core.Logging;

namespace YooPoon.Common.WC.Menu
{
    public class WeChatWeChatMenuService : IWeChatMenuService
    {
        private readonly IWCHelper _wcHelper;
        private readonly IWCCommonService _wcCommonService;
        private readonly ILog _log;

        public WeChatWeChatMenuService(IWCHelper wcHelper, IWCCommonService wcCommonService, ILog log)
        {
            _wcHelper = wcHelper;
            _wcCommonService = wcCommonService;
            _log = log;
        }

        public bool CreateMenu(NormalMenu model,out string msg)
        {
            msg = "";
            var jsonSetting = new JsonSerializerSettings
            {
                ContractResolver = new LowercaseResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

            var result =
                _wcHelper.SendPost(
                    "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + _wcCommonService.AccessToken,
                    JsonConvert.SerializeObject(model,jsonSetting), "application/json; encoding=utf-8");
            var resultModel = JsonConvert.DeserializeObject<MessageResultModel>(result);
            if (resultModel.Errcode != 0)
            {
                msg =string.Format("创建自定义菜单出错，错误代码{0}，错误信息：{1}",resultModel.Errcode,resultModel.ErrMsg);
                _log.Error(msg);
                return false;
            }
            return true;
        }

        public MenuListModel GetMenuList()
        {
            var requestParam = new Dictionary<string, string>
            {
                {"access_token", _wcCommonService.AccessToken}
            };
            var result = _wcHelper.SendGet("https://api.weixin.qq.com/cgi-bin/menu/get", requestParam);
            var model = JsonConvert.DeserializeObject<MenuListModel>(result);
            if (model.Menu == null)
            {
                var error = JsonConvert.DeserializeObject<MessageResultModel>(result);
                _log.Error("获取自定义菜单出错，错误代码{0}，错误信息：{1}", error.Errcode, error.ErrMsg);
                return null;
            }
            return model;
        }

        public bool DeleteMenu(out string msg)
        {
            msg = "";
            var requestParam = new Dictionary<string, string>
            {
                {"access_token", _wcCommonService.AccessToken}
            };
            var result = _wcHelper.SendGet("https://api.weixin.qq.com/cgi-bin/menu/delete", requestParam);
            var resultModel = JsonConvert.DeserializeObject<MessageResultModel>(result);
            if (resultModel.Errcode != 0)
            {
                msg = string.Format("删除自定义菜单出错，错误代码{0}，错误信息：{1}", resultModel.Errcode, resultModel.ErrMsg);
                _log.Error(msg);
                return false;
            }
            return true;
        }
    }
}