using System;
using System.Collections.Generic;

namespace YooPoon.Core.Data
{
    /// <summary>
    /// 数据连接配置
    /// </summary>
    public class DataSettings
    {
        public DataSettings()
        {
            RawDataSettings = new Dictionary<string, string>();
        }

        /// <summary>
        /// 数据源
        /// </summary>
        public string DataProvider { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string DataConnectionString { get; set; }

        /// <summary>
        /// 每行的配置信息
        /// </summary>
        public IDictionary<string, string> RawDataSettings { get; private set; }

        /// <summary>
        /// 初步验证配置信息是否齐全
        /// </summary>
        /// <returns>信息全:True|信息不全:False</returns>
        public bool IsValid()
        {
            return !String.IsNullOrEmpty(this.DataProvider) && !String.IsNullOrEmpty(this.DataConnectionString);
        }
    }
}
