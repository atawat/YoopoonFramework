using YooPoon.Core.Data;

namespace YooPoon.Core.Site
{
    public interface IWorkContext
    {
        IUser CurrentUser { get; set; }
    }
}