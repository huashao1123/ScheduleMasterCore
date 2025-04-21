using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hos.ScheduleMaster.Core.Models
{
    [Table("schedule_groups")]
    public class ScheduleGroupEntity : IEntity
    {
        public ScheduleGroupEntity()
        {
            this.Id = Guid.NewGuid();
            this.CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 分组ID
        /// </summary>
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// 分组描述
        /// </summary>
        [MaxLength(200)]
        [Column("description")]
        public string Description { get; set; }

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
        /// 是否启用
        /// </summary>
        [Column("is_enabled")]
        public bool IsEnabled { get; set; } = true;
    }
} 