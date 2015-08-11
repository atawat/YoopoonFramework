using System;
using YooPoon.Core.Autofac;

namespace YooPoon.Core.Logging
{
    public interface ILog:ISingletonDependency
    {
        void Debug(string message);

        void Information(string message);

        void Warning(string message);

        void Error(string message);

        void Fatal(string message);


        void Debug(Exception exception, string message);

        void Information(Exception exception, string message);

        void Warning(Exception exception, string message);

        void Error(Exception exception, string message);

        void Fatal(Exception exception, string message);


        void Debug(string format, params object[] args);

        void Information(string format, params object[] args);

        void Warning(string format, params object[] args);

        void Error(string format, params object[] args);

        void Fatal(string format, params object[] args);
    }
}