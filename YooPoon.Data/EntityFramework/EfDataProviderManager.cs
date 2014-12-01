using System;
using YooPoon.Core.Data;

namespace YooPoon.Data.EntityFramework
{
    public class EfDataProviderManager:BaseDataProviderManager
    {
        public EfDataProviderManager(DataSettings settings) : base(settings)
        {
            
        }

        public override IDataProvider LoadDataProvider()
        {
            var providerName = Settings.DataProvider;
            if (String.IsNullOrWhiteSpace(providerName))
                throw new Exception("数据配置中没有provider名称");

            switch (providerName.ToLowerInvariant())
            {
                case "sqlserver":
                    return new SqlServerDataProvider();
                default:
                    throw new Exception(string.Format("没有可供支持的provider: {0}", providerName));
            }
        }
    }
}