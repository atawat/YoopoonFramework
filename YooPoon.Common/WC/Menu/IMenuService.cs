using YooPoon.Core.Autofac;

namespace YooPoon.Common.WC.Menu
{
    /// <summary>
    /// 自定义菜单服务
    /// </summary>
    public interface IWeChatMenuService : IDependency
    {
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool CreateMenu(NormalMenu model,out string msg);
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        MenuListModel GetMenuList();
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <returns></returns>
        bool DeleteMenu(out string msg);
    }
}