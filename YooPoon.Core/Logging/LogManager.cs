using System;

namespace YooPoon.Core.Logging
{
    /// <summary>
    /// 实现ILog功能(采用Log.config配置)
    /// </summary>
    public class LogManager:ILog
    {
        private readonly log4net.ILog _log;
        private const string LogName = "APP_Log";

        public LogManager()
        {
            //此处默认使用一个名为APP_Log的Log实体，以后再做多个Log实体的改进
            _log = log4net.LogManager.GetLogger(LogName);
        }
        public void Debug(string message)
        {
            _log.Debug(message);
        }

        public void Information(string message)
        {
            _log.Info(message);
        }

        public void Warning(string message)
        {
            _log.Warn(message);
        }

        public void Error(string message)
        {
            _log.Error(message);
        }

        public void Fatal(string message)
        {
            _log.Fatal(message);
        }

        public void Debug(Exception exception, string message)
        {
            _log.Debug(message,exception);
        }

        public void Information(Exception exception, string message)
        {
            _log.Info(message, exception);
        }

        public void Warning(Exception exception, string message)
        {
            _log.Warn(message, exception);
        }

        public void Error(Exception exception, string message)
        {
            _log.Error(message, exception);
        }

        public void Fatal(Exception exception, string message)
        {
            _log.Fatal(message, exception);
        }

        public void Debug(string format, params object[] args)
        {
            _log.DebugFormat(format,args);
        }

        public void Information(string format, params object[] args)
        {
           _log.InfoFormat(format,args);
        }

        public void Warning(string format, params object[] args)
        {
            _log.WarnFormat(format, args);
        }

        public void Error(string format, params object[] args)
        {
            _log.ErrorFormat(format, args);
        }

        public void Fatal(string format, params object[] args)
        {
            _log.FatalFormat(format, args);
        }
    }
}