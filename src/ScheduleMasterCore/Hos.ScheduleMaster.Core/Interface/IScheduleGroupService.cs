using Hos.ScheduleMaster.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hos.ScheduleMaster.Core.Interface
{
    public interface IScheduleGroupService
    {
        /// <summary>
        /// 创建任务分组
        /// </summary>
        Task<ServiceResponseMessage> CreateGroupAsync(string name, string description, int userId, string userName);

        /// <summary>
        /// 更新任务分组
        /// </summary>
        Task<ServiceResponseMessage> UpdateGroupAsync(Guid groupId, string name, string description);

        /// <summary>
        /// 删除任务分组
        /// </summary>
        Task<ServiceResponseMessage> DeleteGroupAsync(Guid groupId);

        /// <summary>
        /// 获取任务分组
        /// </summary>
        Task<ServiceResponseMessage> GetGroupByIdAsync(Guid groupId);

        /// <summary>
        /// 获取所有任务分组
        /// </summary>
        Task<ServiceResponseMessage> GetAllGroupsAsync();

        /// <summary>
        /// 添加任务到分组
        /// </summary>
        Task<ServiceResponseMessage> AddScheduleToGroupAsync(Guid groupId, Guid scheduleId, int userId, string userName);

        /// <summary>
        /// 从分组中移除任务
        /// </summary>
        Task<ServiceResponseMessage> RemoveScheduleFromGroupAsync(Guid groupId, Guid scheduleId);

        /// <summary>
        /// 获取分组中的所有任务
        /// </summary>
        Task<ServiceResponseMessage> GetGroupSchedulesAsync(Guid groupId);

        /// <summary>
        /// 获取任务所属的所有分组
        /// </summary>
        Task<ServiceResponseMessage> GetScheduleGroupsAsync(Guid scheduleId);
    }
} 