using System.Data.Common;

namespace YooPoon.Core.Data
{
    /// <summary>
    /// Data provider接口
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// 初始化连接工厂
        /// </summary>
        void InitConnectionFactory();

        /// <summary>
        /// 数据库初始化配置
        /// </summary>
        void SetDatabaseInitializer();

        /// <summary>
        /// 初始化数据库
        /// </summary>
        void InitDatabase();

        /// <summary>
        /// 是否支持存储过程
        /// </summary>
        bool StoredProceduredSupported { get; }

        /// <summary>
        /// 获取条件参数 (使用存储过程时)
        /// </summary>
        /// <returns>DbParameter</returns>
        DbParameter GetParameter();
    }
}