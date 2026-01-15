# ColdDream 项目改进计划

经过对项目的全面审查，我发现这是一个功能完备的 MVP（最小可行性产品），包含 .NET 8 后端和 UniApp 前端。为了使其更接近生产级标准，我建议进行以下改进。

## 1. 后端架构优化 (Backend)

### 1.1 引入 DTO (Data Transfer Object) 模式
目前控制器（如 `ProductsController`）直接暴露数据库实体 (`Product`)。这会导致过度暴露内部数据，并产生安全隐患（如批量赋值攻击）。
- **计划**:
    - 创建 `DTOs/Product/ProductDto.cs` (用于返回数据) 和 `CreateProductDto.cs` (用于接收请求)。
    - 配置 `AutoMapper` 映射文件。
    - 重构 `ProductsController` 和 `ProductService` 以使用 DTO。

### 1.2 全局异常处理 (Global Exception Handling)
目前缺乏统一的错误处理机制。
- **计划**:
    - 创建中间件 `Middleware/GlobalExceptionMiddleware.cs`。
    - 统一捕获异常并返回标准化的错误响应结构（如 `{ "statusCode": 500, "message": "..." }`）。
    - 在 `Program.cs` 中注册该中间件。

### 1.3 规范化 API 响应
API 返回的格式不统一（有时直接返回对象，有时返回字符串）。
- **计划**:
    - 定义泛型响应包装类 `ApiResponse<T>`。
    - 确保所有 API 返回统一格式：`{ "success": true, "data": ..., "message": null }`。

## 2. 前端工程化改进 (Frontend)

### 2.1 环境变量配置
`request.ts` 中硬编码了 `http://localhost:5116/api`。
- **计划**:
    - 在 `ColdDream.UniApp` 根目录创建 `.env` 文件。
    - 使用 `import.meta.env.VITE_API_BASE_URL` 动态获取 API 地址。

### 2.2 增强网络请求封装
目前的请求封装对 401 (未授权) 等状态码处理不够完善。
- **计划**:
    - 修改 `utils/request.ts`。
    - 增加拦截器逻辑：当检测到 401 状态码时，自动清除本地 Token 并跳转至登录页。

## 3. 代码质量与规范

### 3.1 移除硬编码字符串
后端代码中存在一些硬编码字符串（如 CORS 策略名、错误信息）。
- **计划**:
    - 将常量提取到 `Constants` 类或配置文件中。

---

**建议执行顺序**:
1.  **后端 DTO 重构** (最基础的架构改动)
2.  **前端环境变量配置** (解耦开发环境)
3.  **全局异常处理** (提升稳定性)

请确认是否开始执行此计划？我们可以先从 **后端 DTO 重构** 开始。