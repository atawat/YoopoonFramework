using System;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using YooPoon.WebFramework.Authentication.Entity;

namespace YooPoon.WebFramework.Authentication.Services
{
    public class ControllerActionService:IControllerActionService
    {
        private readonly IRepository<ControllerAction> _actionRepository;
        private readonly ILog _log;

        public ControllerActionService(IRepository<ControllerAction> actionRepository,ILog log)
        {
            _actionRepository = actionRepository;
            _log = log;
        }
        public ControllerAction GetControllerAction(int id)
        {
            try
            {
                return _actionRepository.GetById(id);
            }
            catch (Exception e)
            {
                _log.Error(e,"获取Action出错");
                return null;
            }
        }

        public ControllerAction GetControllerActionByName(string controllerName, string actionName)
        {
            try
            {
                return _actionRepository.Get(c => c.ControllerName == controllerName && c.ActionName == actionName);
            }
            catch (Exception e)
            {
                _log.Error(e,"获取Action出错");
                return null;
            }
        }

        public ControllerAction ExistOrCreate(string controllerName, string actionName)
        {
            try
            {
                var action = _actionRepository.Get(c => c.ActionName == actionName && c.ControllerName == controllerName);
                if (action != null)
                    return action;
                var newAction = new ControllerAction
                {
                    ActionName = actionName,
                    ControllerName = controllerName
                };
                _actionRepository.Insert(newAction);
                return newAction;
            }
            catch (Exception e)
            {
                _log.Error(e,"");
                return null;
            }
        }

        public ControllerAction Update(ControllerAction controllerAction)
        {
            try
            {
                _actionRepository.Update(controllerAction);
                return controllerAction;
            }
            catch (Exception e)
            {
                _log.Error(e,"更新Action失败");
                return null;
            }
        }

        public ControllerAction Create(ControllerAction controllerAction)
        {
            try
            {
                _actionRepository.Insert(controllerAction);
                return controllerAction;
            }
            catch (Exception e)
            {
                _log.Error(e,"创建Action失败");
                return null;
            }
        }

        public bool Delet(ControllerAction controllerAction)
        {
            try
            {
                _actionRepository.Delete(controllerAction);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e,"删除Action失败");
                return false;
            }
        }
    }
}