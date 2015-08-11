using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using YooPoon.WebFramework.User.Entity;

namespace YooPoon.WebFramework.User.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<UserBase> _userRepository;
        private readonly ILog _log;

        public UserService(IRepository<UserBase> userRepository,ILog log)
        {
            _userRepository = userRepository;
            _log = log;
        }
        public UserBase InsertUser(UserBase user)
        {
            try
            {
                _userRepository.Insert(user);
                return user;
            }
            catch (Exception e)
            {
                _log.Error(e,"添加用户失败");
                return null;
            }

        }


        public bool DeleteUser(UserBase user)
        {
            try
            {
                _userRepository.Delete(user);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "删除用户失败");
                return false;
            }


        }

        public UserBase FindUser(int id)
        {
            try
            {
                return _userRepository.GetById(id);
            }
            catch (Exception e)
            {
                _log.Error(e, "查找用户失败");
                return null;
            }
        }

        public bool ModifyUser(UserBase user)
        {
            try
            {
                _userRepository.Update(user);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "更新用户失败");
                return false;
            }
        }

        public UserBase GetUserByName(string username)
        {
            try
            {
                return _userRepository.Table.FirstOrDefault(u => u.UserName == username);
            }
            catch (Exception e)
            {
                _log.Error(e, "获取用户失败");
                return null;
            }
        }
        public UserBase GetUserById(int id)
        {
            try
            {
                return _userRepository.Table.FirstOrDefault(u => u.Id  == id);
            }
            catch (Exception e)
            {
                _log.Error(e, "获取用户失败");
                return null;
            }
        }

        public IQueryable<UserBase> GetUserByCondition(UserSearchCondition condition)
        {
            try
            {
                var query = _userRepository.Table;
                if (condition.BeginTime.HasValue)
                {
                    query = query.Where(c => c.RegTime >= condition.BeginTime.Value);
                }
                if (condition.EndTime.HasValue)
                {
                    query = query.Where(c => c.RegTime < condition.EndTime.Value);
                }
                if (condition.Status.HasValue)
                {
                    query = query.Where(c => c.Status == condition.Status);
                }
                if (condition.Ids != null && condition.Ids.Length > 0)
                {
                    query = query.Where(c => condition.Ids.Contains(c.Id));
                }
                if (!string.IsNullOrEmpty(condition.UserName))
                {
                    query = query.Where(c => c.UserName.Contains(condition.UserName));
                }
                if (condition.OrderBy.HasValue)
                {
                    switch (condition.OrderBy)
                    {
                        case EnumUserOrderBy.Default:
                            query = condition.IsDescending
                                ? query.OrderByDescending(c => c.Id)
                                : query.OrderBy(c => c.Id);
                            break;
                        case EnumUserOrderBy.ById:
                            query = condition.IsDescending
                                ? query.OrderByDescending(c => c.Id)
                                : query.OrderBy(c => c.Id);
                            break;
                        case EnumUserOrderBy.ByName:
                            query = condition.IsDescending
                                ? query.OrderByDescending(c => c.UserName)
                                : query.OrderBy(c => c.UserName);
                            break;
                        case EnumUserOrderBy.RegTime:
                            query = condition.IsDescending
                                ? query.OrderByDescending(c => c)
                                : query.OrderBy(c => c.RegTime);
                            break;
                    }
                }
                else
                {
                    query = condition.IsDescending
                                ? query.OrderByDescending(c => c.Id)
                                : query.OrderBy(c => c.Id);
                }
                if (condition.Page.HasValue && condition.PageSize.HasValue)
                {
                    query =
                        query.Skip((condition.Page.Value - 1) * condition.PageSize.Value).Take(condition.PageSize.Value);
                }
                return query;
            }
            catch (Exception e)
            {
                _log.Error(e, "获取用户列表失败");
                return null;
            }
        }

        public int GetUserCountByCondition(UserSearchCondition condition)
        {
            try
            {
                var query = _userRepository.Table;
                if (condition.BeginTime.HasValue)
                {
                    query = query.Where(c => c.RegTime >= condition.BeginTime.Value);
                }
                if (condition.EndTime.HasValue)
                {
                    query = query.Where(c => c.RegTime < condition.EndTime.Value);
                }
                if (condition.Status.HasValue)
                {
                    query = query.Where(c => c.Status == condition.Status);
                }
                if (condition.Ids != null && condition.Ids.Length > 0)
                {
                    query = query.Where(c => condition.Ids.Contains(c.Id));
                }
                if (!string.IsNullOrEmpty(condition.UserName))
                {
                    query = query.Where(c => c.UserName.Contains(condition.UserName));
                }
                return query.Count();
            }
            catch (Exception e)
            {
                _log.Error(e, "获取用户列表失败");
                return -1;
            }
        }
    }
}