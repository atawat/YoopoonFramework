using System;

namespace YooPoon.WebFramework.User.Entity
{
    public class UserSearchCondition
    {
        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int[] Ids { get; set; }
        public string UserName { get; set; }

        public int? Status { get; set; }

        public EnumUserOrderBy? OrderBy { get; set; }

        public bool IsDescending { get; set; }

        public int? PageSize { get; set; }

        public int? Page { get; set; }
    }

    public enum EnumUserOrderBy
    {
        Default = 0,
        ById = 1,
        ByName = 2,
        RegTime = 3
    }
}