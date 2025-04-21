using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hos.ScheduleMaster.Core.Models
{
    [Table("schedule_group_relations")]
    public class ScheduleGroupRelationEntity : IEntity
    {
        public ScheduleGroupRelationEntity()
        {
            this.Id = Guid.NewGuid();
            this.CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 关联ID
        /// </summary>
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// 分组ID
        /// </summary>
        [Required]
        [Column("group_id")]
        public Guid GroupId { get; set; }

        /// <summary>
        /// 任务ID
        /// </summary>
        [Required]
        [Column("schedule_id")]
        public Guid ScheduleId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("create_time")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [Column("create_user_id")]
        public int CreateUserId { get; set; }

        /// <summary>
        /// 创建人账号
        /// </summary>
        [Column("create_user_name")]
        [MaxLength(50)]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 导航属性 - 分组
        /// </summary>
        [ForeignKey("GroupId")]
        public virtual ScheduleGroupEntity Group { get; set; }

        /// <summary>
        /// 导航属性 - 任务
        /// </summary>
        [ForeignKey("ScheduleId")]
        public virtual ScheduleEntity Schedule { get; set; }
    }
} 