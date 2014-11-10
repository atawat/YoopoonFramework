using YooPoon.Core.Data;

namespace YooPoon.Framework.Authentication.Entity
{
    public class ControllerAction:IBaseEntity
    {
        public int Id { get; set; }

        public string ActionName { get; set; }

        public string ControllerName { get; set; }
    }
}