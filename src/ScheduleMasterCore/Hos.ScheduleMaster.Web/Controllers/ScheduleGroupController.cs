using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hos.ScheduleMaster.Core.Models;
using Hos.ScheduleMaster.Core.Services;
using Hos.ScheduleMaster.Web.Filters;
using Hos.ScheduleMaster.Core;
using Hos.ScheduleMaster.Core.Interface;

namespace Hos.ScheduleMaster.Web.Controllers
{
    [Authorize]
    public class ScheduleGroupController : AdminController
    {
        [Autowired]
        public IScheduleGroupService _scheduleGroupService { get; set; }

       

        /// <summary>
        /// 分组列表页面
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取分组列表
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllGroups()
        {
            var result = await _scheduleGroupService.GetAllGroupsAsync();
            if (result.Success)
            {
                var groups = (List<ScheduleGroupEntity>)result.Data;
                return Json(new { 
                    total = groups.Count, 
                    rows = groups.Select(m => new {
                        id = m.Id,
                        name = m.Name,
                        description = m.Description,
                        createUserName = m.CreateUserName,
                        createTime = m.CreateTime
                    })
                });
            }
            return Json(new { total = 0, rows = new List<object>() });
        }

        /// <summary>
        /// 获取分组详情
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDetail(Guid id)
        {
            var group = await _scheduleGroupService.GetGroupByIdAsync(id);
            if (group == null)
            {
                return Json(new ServiceResponseMessage { Status = 0, Message = "分组不存在" });
            }
            return Json(new ServiceResponseMessage { Status = ResultStatus.Success, Data = group });
        }

        /// <summary>
        /// 创建分组
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGroupRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new ServiceResponseMessage { Status = ResultStatus.Failed, Message = "数据验证失败" });
            }
            var result = await _scheduleGroupService.CreateGroupAsync(request.Name, request.Description, CurrentAdmin.Id, CurrentAdmin.UserName);
            if (result.Success)
            {
                return Json(new ServiceResponseMessage { Status = ResultStatus.Success, Message = "创建成功" });
            }
            return Json(new ServiceResponseMessage { Status = 0, Message = "创建失败" });
        }

        /// <summary>
        /// 更新分组
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UpdateGroupRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new ServiceResponseMessage { Status = ResultStatus.Failed, Message = "数据验证失败" });
            }

            var result = await _scheduleGroupService.UpdateGroupAsync(request.Id, request.Name, request.Description);
            if (result.Success)
            {
                return Json(new ServiceResponseMessage { Status = ResultStatus.Success, Message = "更新成功" });
            }
            return Json(new ServiceResponseMessage { Status = 0, Message = "更新失败" });
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _scheduleGroupService.DeleteGroupAsync(id);
            if (result.Success)
            {
                return Json(new ServiceResponseMessage { Status = ResultStatus.Success, Message = "删除成功" });
            }
            return Json(new ServiceResponseMessage { Status = 0, Message = "删除失败" });
        }

        /// <summary>
        /// 获取分组中的任务列表
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetGroupSchedules(Guid groupId)
        {
            var result = await _scheduleGroupService.GetGroupSchedulesAsync(groupId);
            if (result.Success)
            {
                var schedules = (List<ScheduleEntity>)result.Data;
                return Json(new { 
                    total = schedules.Count, 
                    rows = schedules.Select(m => new {
                        id = m.Id,
                        title = m.Title,
                        metaType = m.MetaType,
                        status = m.Status
                    })
                });
            }
            return Json(new { total = 0, rows = new List<object>() });
        }

        /// <summary>
        /// 添加任务到分组
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddScheduleToGroup(Guid groupId, Guid scheduleId)
        {
            var result = await _scheduleGroupService.AddScheduleToGroupAsync(groupId, scheduleId, CurrentAdmin.Id, CurrentAdmin.UserName);
            if (result.Success)
            {
                return Json(new ServiceResponseMessage { Status = ResultStatus.Success, Message = "添加成功" });
            }
            return Json(new ServiceResponseMessage { Status = 0, Message = "添加失败" });
        }

        /// <summary>
        /// 从分组中移除任务
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> RemoveScheduleFromGroup(Guid groupId, Guid scheduleId)
        {
            var result = await _scheduleGroupService.RemoveScheduleFromGroupAsync(groupId, scheduleId);
            if (result.Success)
            {
                return Json(new ServiceResponseMessage { Status = ResultStatus.Success, Message = "移除成功" });
            }
            return Json(new ServiceResponseMessage { Status = 0, Message = "移除失败" });
        }
    }

    public class CreateGroupRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateGroupRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
} 