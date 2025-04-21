using Hos.ScheduleMaster.Core.Common;
using Hos.ScheduleMaster.Core.Interface;
using Hos.ScheduleMaster.Core.Models;
using Hos.ScheduleMaster.Core.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hos.ScheduleMaster.Core.Services
{
    [ServiceMapTo(typeof(IScheduleGroupService))]
    public class ScheduleGroupService : BaseService, IScheduleGroupService
    {
       

        /// <summary>
        /// 创建任务分组
        /// </summary>
        public async Task<ServiceResponseMessage> CreateGroupAsync(string name, string description, int userId, string userName)
        {
            // 验证分组名称
            if (string.IsNullOrWhiteSpace(name))
            {
                return ServiceResult(ResultStatus.Failed, "分组名称不能为空");
            }

            // 检查名称是否已存在
            if (await _repositoryFactory.ScheduleGroups.AnyAsync(x => x.Name == name))
            {
                return ServiceResult(ResultStatus.Failed, "分组名称已存在");
            }

            var group = new ScheduleGroupEntity
            {
                Name = name,
                Description = description,
                CreateUserId = userId,
                CreateUserName = userName,
                CreateTime = DateTime.Now,
                IsEnabled = true
            };

            try
            {
                 _repositoryFactory.ScheduleGroups.Add(group);
                if (await _unitOfWork.CommitAsync() > 0)
                {
                    return ServiceResult(ResultStatus.Success, "分组创建成功", group);
                }
                return ServiceResult(ResultStatus.Failed, "分组创建失败");
            }
            catch (Exception ex)
            {
                return ServiceResult(ResultStatus.Failed, $"分组创建失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 更新任务分组
        /// </summary>
        public async Task<ServiceResponseMessage> UpdateGroupAsync(Guid groupId, string name, string description)
        {
            // 验证分组名称
            if (string.IsNullOrWhiteSpace(name))
            {
                return ServiceResult(ResultStatus.Failed, "分组名称不能为空");
            }

            // 检查分组是否存在
            var group = await _repositoryFactory.ScheduleGroups.FirstOrDefaultAsync(x => x.Id == groupId);
            if (group == null)
            {
                return ServiceResult(ResultStatus.Failed, "分组不存在");
            }

            // 检查名称是否已存在（排除自身）
            if (await _repositoryFactory.ScheduleGroups.AnyAsync(x => x.Name == name && x.Id != groupId))
            {
                return ServiceResult(ResultStatus.Failed, "分组名称已存在");
            }

            try
            {
                group.Name = name;
                group.Description = description;
                 _repositoryFactory.ScheduleGroups.Update(group);
                if (await _unitOfWork.CommitAsync() > 0)
                {
                    return ServiceResult(ResultStatus.Success, "分组更新成功");
                }
                return ServiceResult(ResultStatus.Failed, "分组更新失败");
            }
            catch (Exception ex)
            {
                return ServiceResult(ResultStatus.Failed, $"分组更新失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 删除任务分组
        /// </summary>
        public async Task<ServiceResponseMessage> DeleteGroupAsync(Guid groupId)
        {
            // 检查分组是否存在
            var group = await _repositoryFactory.ScheduleGroups.FirstOrDefaultAsync(x => x.Id == groupId);
            if (group == null)
            {
                return ServiceResult(ResultStatus.Failed, "分组不存在");
            }

            try
            {
                // 删除分组关联的任务
                _repositoryFactory.ScheduleGroupRelations.DeleteBy(x => x.GroupId == groupId);
                // 删除分组
                _repositoryFactory.ScheduleGroups.Delete(group);
                if (await _unitOfWork.CommitAsync() > 0)
                {
                    return ServiceResult(ResultStatus.Success, "分组删除成功");
                }
                return ServiceResult(ResultStatus.Failed, "分组删除失败");
            }
            catch (Exception ex)
            {
                return ServiceResult(ResultStatus.Failed, $"分组删除失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 获取任务分组
        /// </summary>
        public async Task<ServiceResponseMessage> GetGroupByIdAsync(Guid groupId)
        {
            var group = await _repositoryFactory.ScheduleGroups.FirstOrDefaultAsync(x => x.Id == groupId);
            if (group == null)
            {
                return ServiceResult(ResultStatus.Failed, "分组不存在");
            }
            return ServiceResult(ResultStatus.Success, "获取成功", group);
        }

        /// <summary>
        /// 获取所有任务分组
        /// </summary>
        public async Task<ServiceResponseMessage> GetAllGroupsAsync()
        {
            var groups = await _repositoryFactory.ScheduleGroups.WhereNoTracking(x => x.IsEnabled).ToListAsync();
            return ServiceResult(ResultStatus.Success, "获取成功", groups);
        }

        /// <summary>
        /// 添加任务到分组
        /// </summary>
        public async Task<ServiceResponseMessage> AddScheduleToGroupAsync(Guid groupId, Guid scheduleId, int userId, string userName)
        {
            // 检查分组是否存在
            var group = await _repositoryFactory.ScheduleGroups.FirstOrDefaultAsync(x => x.Id == groupId);
            if (group == null)
            {
                return ServiceResult(ResultStatus.Failed, "分组不存在");
            }

            // 检查任务是否存在
            var schedule = await _repositoryFactory.Schedules.FirstOrDefaultAsync(x => x.Id == scheduleId);
            if (schedule == null)
            {
                return ServiceResult(ResultStatus.Failed, "任务不存在");
            }

            // 检查是否已经添加过
            if (await _repositoryFactory.ScheduleGroupRelations.AnyAsync(x => x.GroupId == groupId && x.ScheduleId == scheduleId))
            {
                return ServiceResult(ResultStatus.Failed, "任务已在该分组中");
            }

            try
            {
                var relation = new ScheduleGroupRelationEntity
                {
                    GroupId = groupId,
                    ScheduleId = scheduleId,
                    CreateUserId = userId,
                    CreateUserName = userName,
                    CreateTime = DateTime.Now
                };

                _repositoryFactory.ScheduleGroupRelations.Add(relation);
                if (await _unitOfWork.CommitAsync() > 0)
                {
                    return ServiceResult(ResultStatus.Success, "添加成功");
                }
                return ServiceResult(ResultStatus.Failed, "添加失败");
            }
            catch (Exception ex)
            {
                return ServiceResult(ResultStatus.Failed, $"添加失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 从分组中移除任务
        /// </summary>
        public async Task<ServiceResponseMessage> RemoveScheduleFromGroupAsync(Guid groupId, Guid scheduleId)
        {
            var relation = await _repositoryFactory.ScheduleGroupRelations.FirstOrDefaultAsync(x => x.GroupId == groupId && x.ScheduleId == scheduleId);
            if (relation == null)
            {
                return ServiceResult(ResultStatus.Failed, "任务不在该分组中");
            }

            try
            {
                _repositoryFactory.ScheduleGroupRelations.Delete(relation);
                if (await _unitOfWork.CommitAsync() > 0)
                {
                    return ServiceResult(ResultStatus.Success, "移除成功");
                }
                return ServiceResult(ResultStatus.Failed, "移除失败");
            }
            catch (Exception ex)
            {
                return ServiceResult(ResultStatus.Failed, $"移除失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 获取分组中的所有任务
        /// </summary>
        public async Task<ServiceResponseMessage> GetGroupSchedulesAsync(Guid groupId)
        {
            // 检查分组是否存在
            var group = await _repositoryFactory.ScheduleGroups.FirstOrDefaultAsync(x => x.Id == groupId);
            if (group == null)
            {
                return ServiceResult(ResultStatus.Failed, "分组不存在");
            }

            try
            {
                var schedules = await (from r in _repositoryFactory.ScheduleGroupRelations.Table
                                      join s in _repositoryFactory.Schedules.Table on r.ScheduleId equals s.Id
                                      where r.GroupId == groupId
                                      select s).ToListAsync();
                return ServiceResult(ResultStatus.Success, "获取成功", schedules);
            }
            catch (Exception ex)
            {
                return ServiceResult(ResultStatus.Failed, $"获取失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 获取任务所属的所有分组
        /// </summary>
        public async Task<ServiceResponseMessage> GetScheduleGroupsAsync(Guid scheduleId)
        {
            // 检查任务是否存在
            var schedule = await _repositoryFactory.Schedules.FirstOrDefaultAsync(x => x.Id == scheduleId);
            if (schedule == null)
            {
                return ServiceResult(ResultStatus.Failed, "任务不存在");
            }

            try
            {
                var groups = await (from r in _repositoryFactory.ScheduleGroupRelations.Table
                                   join g in _repositoryFactory.ScheduleGroups.Table on r.GroupId equals g.Id
                                   where r.ScheduleId == scheduleId
                                   select g).ToListAsync();
                return ServiceResult(ResultStatus.Success, "获取成功", groups);
            }
            catch (Exception ex)
            {
                return ServiceResult(ResultStatus.Failed, $"获取失败：{ex.Message}");
            }
        }
    }
} 