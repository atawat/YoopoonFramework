using System.Collections.Generic;

namespace YooPoon.Common.WC.Menu
{
    /// <summary>
    /// 菜单基类
    /// </summary>
    public class MenuBaseModel
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public string Url { get; set; }

        public string Media_Url { get; set; }

        public List<MenuBaseModel> Sub_Button { get; set; }
    }

    /// <summary>
    /// 普通菜单
    /// </summary>
    public class NormalMenu
    {
        public List<MenuBaseModel> Button { get; set; }

        public int MenuId { get; set; }
    }

    /// <summary>
    /// 个性化菜单
    /// </summary>
    public class ConditionalMenu : NormalMenu
    {
        public object MatchRule { get; set; }
    }

    /// <summary>
    /// 菜单列表对象
    /// </summary>
    public class MenuListModel
    {
        public NormalMenu Menu { get; set; }

        public List<ConditionalMenu> ConditionalMenu { get; set; }
    }
}