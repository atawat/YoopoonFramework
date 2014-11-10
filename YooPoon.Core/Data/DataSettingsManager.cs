using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace YooPoon.Core.Data
{
    public  class DataSettingsManager
    {
        protected const char Separator = ':';
        protected const string Filename = "Settings.txt";

        /// <summary>
        /// 将虚拟路径转换为物理路径
        /// </summary>
        /// <param name="path">需要转换的路径. E.g. "~/bin"</param>
        /// <returns>物理路径. E.g. "c:\inetpub\wwwroot\bin"</returns>
        protected virtual string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                //是ASP.NET Web应用程序
                return HostingEnvironment.MapPath(path);
            }
            //不是ASP.NET Web应用程序.比如单元测试时
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }

        /// <summary>
        /// 解析配置文件
        /// </summary>
        /// <param name="text">配置文本</param>
        /// <returns>DataSettings对象</returns>
        protected virtual DataSettings ParseSettings(string text)
        {
            var shellSettings = new DataSettings();
            if (String.IsNullOrEmpty(text))
                return shellSettings;

            var settings = new List<string>();
            using (var reader = new StringReader(text))
            {
                string str;
                while ((str = reader.ReadLine()) != null)
                    settings.Add(str);
            }

            foreach (var setting in settings)
            {
                var separatorIndex = setting.IndexOf(Separator);
                if (separatorIndex == -1)
                {
                    continue;
                }
                string key = setting.Substring(0, separatorIndex).Trim();
                string value = setting.Substring(separatorIndex + 1).Trim();

                switch (key)
                {
                    case "DataProvider":
                        shellSettings.DataProvider = value;
                        break;
                    case "DataConnectionString":
                        shellSettings.DataConnectionString = value;
                        break;
                    default:
                        shellSettings.RawDataSettings.Add(key, value);
                        break;
                }
            }

            return shellSettings;
        }

        /// <summary>
        /// 构建配置对象
        /// </summary>
        /// <param name="settings">配置对象</param>
        /// <returns>配置文本</returns>
        protected virtual string ComposeSettings(DataSettings settings)
        {
            if (settings == null)
                return "";

            return string.Format("DataProvider: {0}{2}DataConnectionString: {1}{2}",
                                 settings.DataProvider,
                                 settings.DataConnectionString,
                                 Environment.NewLine
                );
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="filePath">配置文件路径; 传null以使用默认路径</param>
        /// <returns></returns>
        public virtual DataSettings LoadSettings(string filePath = null)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                //单元测试时，使用webHelper.MapPath替代HostingEnvironment.MapPath
                filePath = Path.Combine(MapPath("~/App_Data/"), Filename);
            }
            if (File.Exists(filePath))
            {
                string text = File.ReadAllText(filePath);
                return ParseSettings(text);
            }
            return new DataSettings();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="settings">DataSettings对象</param>
        public virtual void SaveSettings(DataSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            //单元测试时，使用webHelper.MapPath替代HostingEnvironment.MapPath
            string filePath = Path.Combine(MapPath("~/App_Data/"), Filename);
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath))
                {
                    
                }
            }

            var text = ComposeSettings(settings);
            File.WriteAllText(filePath, text);
        }
    }
}