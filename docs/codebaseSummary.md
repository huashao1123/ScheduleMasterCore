# ScheduleMaster 代码库概述

## 项目结构
```
src/
├── ScheduleMasterCore/
│   ├── Hos.ScheduleMaster.Base/        # 基础类库
│   ├── Hos.ScheduleMaster.Core/        # 核心业务逻辑
│   ├── Hos.ScheduleMaster.Web/         # Web管理界面
│   ├── Hos.ScheduleMaster.QuartzHost/  # 任务调度主机
│   ├── Hos.ScheduleMaster.Demo/        # 示例项目
│   └── Hos.ScheduleMaster.xUnitTest/   # 单元测试
└── db/                                  # 数据库脚本
```

## 核心组件说明

### 1. Hos.ScheduleMaster.Base
基础类库，提供通用功能和工具类：
- 通用工具类
- 基础接口定义
- 常量定义
- 扩展方法
- 通用模型
- 统一响应格式（ServiceResponseMessage）

### 2. Hos.ScheduleMaster.Core
核心业务逻辑实现：
- 任务调度逻辑
- 节点管理
- 用户认证
- 权限控制
- 数据访问
- 业务模型
- 任务分组管理
  - ScheduleGroupService：分组管理服务
  - ScheduleGroupController：分组管理控制器
  - ScheduleGroup：分组数据模型
  - ScheduleGroupSchedule：分组与任务关联模型
- 统一响应格式（ServiceResponseMessage）

### 3. Hos.ScheduleMaster.Web
Web管理界面：
- 控制器
- 视图
  - Schedule/Index.cshtml：任务列表页面
  - Schedule/Detail.cshtml：任务详情页面
    - 基本信息展示
    - HTTP配置（针对HTTP任务）
    - 运行节点列表
    - 自定义参数
    - 监护人列表
    - 后置任务列表
    - 面包屑导航（任务列表-详情）
    - 返回按钮
  - Schedule/Edit.cshtml：任务编辑页面
- 静态资源
- API接口
- 中间件
- 过滤器
- 任务分组管理界面
  - Index.cshtml：分组管理主页面
  - 分组创建/编辑模态框
  - 分组任务列表模态框
  - 添加任务模态框
  - 分组管理JavaScript代码

### 4. Hos.ScheduleMaster.QuartzHost
任务调度主机：
- 任务执行器
- 调度器
- 资源管理
- 状态监控
- 日志记录

### 5. Hos.ScheduleMaster.Demo
示例项目：
- 示例任务
- 使用说明
- 最佳实践
- 配置示例

### 6. Hos.ScheduleMaster.xUnitTest
单元测试项目：
- 单元测试
- 集成测试
- 测试工具
- 测试数据

## 数据流
1. **任务创建流程**
   - Web界面创建任务
   - 保存任务配置
   - 分配执行节点
   - 更新调度计划

2. **任务执行流程**
   - Master节点调度任务
   - Worker节点接收任务
   - 执行任务
   - 记录执行结果
   - 更新任务状态

3. **节点管理流程**
   - Worker节点注册
   - 心跳检测
   - 负载均衡
   - 故障转移

4. **任务分组管理流程**
   - 创建任务分组
   - 添加任务到分组
   - 从分组中移除任务
   - 查看分组任务列表
   - 管理分组信息
   - 统一响应格式处理

## 外部依赖
### NuGet包
- EntityFrameworkCore
- Quartz
- Newtonsoft.Json
- log4net
- AutoMapper
- Swashbuckle

### 前端依赖
- jQuery
- Bootstrap
- BeyondAdmin
- Moment.js
- DataTables
- Toastr
- Bootstrap-Table

## 配置管理
- appsettings.json
- 数据库配置
- 日志配置
- 节点配置
- 任务配置

## 安全机制
- 身份认证
- 权限控制
- 数据加密
- 访问控制
- 审计日志

## 扩展点
1. **任务类型扩展**
   - 自定义任务类型
   - 任务参数配置
   - 执行逻辑定制

2. **存储扩展**
   - 自定义数据源
   - 数据访问定制
   - 缓存策略

3. **监控扩展**
   - 自定义监控指标
   - 告警规则
   - 报表定制

4. **分组管理扩展**
   - 自定义分组属性
   - 分组操作定制
   - 分组权限控制

## 开发规范
1. **代码规范**
   - 命名规范
   - 注释规范
   - 代码格式
   - 最佳实践

2. **Git工作流**
   - 分支管理
   - 提交规范
   - 版本控制
   - 代码审查

3. **文档规范**
   - 接口文档
   - 使用说明
   - 部署文档
   - 维护文档

## 部署说明
1. **环境要求**
   - .NET Core 3.1
   - 数据库支持
   - 操作系统要求
   - 网络要求

2. **部署步骤**
   - 环境准备
   - 配置修改
   - 数据库初始化
   - 服务启动
   - 验证部署

## 维护指南
1. **日常维护**
   - 日志检查
   - 性能监控
   - 数据备份
   - 安全更新

2. **故障处理**
   - 故障诊断
   - 应急处理
   - 恢复流程
   - 预防措施

## 统一响应格式
### ServiceResponseMessage
```csharp
public class ServiceResponseMessage
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
    public int Code { get; set; }
}
```

### 使用场景
- API接口响应
- 业务操作结果
- 错误信息返回
- 数据查询结果
- 任务分组管理操作

### 优势
- 统一的响应结构
- 清晰的状态表示
- 灵活的数据封装
- 便于前端处理
- 简化错误处理逻辑 