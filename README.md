# ColdDream - 全栈旅游预订平台

ColdDream 是一个现代化的全栈旅游预订平台，旨在为用户提供从旅行灵感发现、路线规划到在线预订的一站式服务。项目集成了 Web 端、移动端 (UniApp) 以及强大的后端 API 服务。

## 🌟 核心功能

### 🗺️ 旅游路线 (Tours)
- **精选路线**: 浏览经过精心策划的旅游路线，支持按目的地、价格、热度筛选。
- **路线详情**: 查看详细的每日行程安排、费用包含说明及预订须知。
- **智能搜索**: 快速查找心仪的旅游目的地。

### 📚 攻略社区 (Guide & Community)
- **达人攻略**: 浏览旅游达人分享的深度游记和实用指南。
- **发布与分享**: 用户可以发布自己的旅行攻略，支持图文混排。
- **互动交流**: 支持查看和筛选社区内容。

### 💡 旅行灵感 (Inspiration)
- **灵感瀑布流**: 探索高质量的旅行美图和短内容。
- **社交互动**: 支持点赞、收藏喜欢的灵感内容。
- **内容发布**: 分享你的旅行瞬间。

### 📅 在线预订 (Booking)
- **便捷下单**: 选择出行日期、人数，系统自动计算价格。
- **订单管理**: 完整的订单状态追踪（待确认、已支付、已完成、已取消）。
- **优惠系统**: 支持使用优惠券抵扣金额。

### ✨ 私人定制 (Custom Tour)
- **个性化需求**: 提交目的地、预算、天数等需求，获取专属定制方案。
- **进度追踪**: 实时查看定制需求的受理状态。

### 👤 用户中心 (Dashboard)
- **个人管理**: 修改个人资料、头像。
- **我的订单**: 统一管理所有预订订单。
- **我的收藏**: 查看收藏的路线和灵感。

---

## 🛠️ 技术栈

### 后端服务 (ColdDream.Api)
- **框架**: .NET 8.0 (ASP.NET Core Web API)
- **数据库**: SQL Server (Entity Framework Core 8.0)
- **认证**: JWT Bearer + ASP.NET Core Identity
- **缓存**: Redis
- **主要库**: 
  - `AutoMapper`: 对象映射
  - `Serilog`: 结构化日志记录
  - `Swashbuckle`: Swagger API 文档生成

### Web 前端 (ColdDream.Web)
- **框架**: Vue 3 (Composition API) + TypeScript
- **构建工具**: Vite
- **状态管理**: Pinia
- **路由**: Vue Router
- **UI 框架**: Tailwind CSS
- **HTTP**: Axios

### 移动端 (ColdDream.UniApp)
- **框架**: UniApp (基于 Vue 3 + TypeScript)
- **平台**: 支持编译为微信小程序、H5 及 App

---

## 🚀 快速开始

### 前置要求
- Node.js 18+
- .NET 8.0 SDK
- SQL Server
- Redis (可选)

### 1. 后端启动
```bash
cd ColdDream.Api
# 更新数据库
dotnet ef database update
# 启动服务
dotnet run
```
服务默认运行在 `https://localhost:7001` (HTTPS) 或 `http://localhost:5000` (HTTP)。

### 2. Web 前端启动
```bash
cd ColdDream.Web
# 安装依赖
npm install
# 启动开发服务器
npm run dev
```
访问 `http://localhost:5173` 查看页面。

### 3. 移动端启动
使用 HBuilderX 打开 `ColdDream.UniApp` 目录进行运行和调试。

---

## � 目录结构

```
ColdDream/
├── ColdDream.Api/          # 后端 API 项目
│   ├── Controllers/        # API 控制器
│   ├── Models/             # 实体模型
│   ├── Data/               # 数据库上下文
│   └── Services/           # 业务逻辑服务
│
├── ColdDream.Web/          # Web 前端项目
│   ├── src/
│   │   ├── api/            # API 接口定义
│   │   ├── views/          # 页面组件
│   │   ├── stores/         # Pinia 状态管理
│   │   └── router/         # 路由配置
│
└── ColdDream.UniApp/       # 移动端项目
```

## � 许可证

MIT License
