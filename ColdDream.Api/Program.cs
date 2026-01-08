using System.Text;
using ColdDream.Api.Data;
using ColdDream.Api.Models;
using ColdDream.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    );
});

// Add DbContext
// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add Redis
builder.Services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(sp =>
    StackExchange.Redis.ConnectionMultiplexer.Connect(
        builder.Configuration.GetConnectionString("Redis")!
    )
);

// Add Identity
builder
    .Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add AuthService
builder.Services.AddScoped<IAuthService, AuthService>();

// Add TourRouteService
builder.Services.AddScoped<ITourRouteService, TourRouteService>();

// Add ButlerService
builder.Services.AddScoped<IButlerService, ButlerService>();

// Add BookingService
builder.Services.AddScoped<IBookingService, BookingService>();

// Add LeaderboardService
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();

// Add ProductService
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IWeChatService, WeChatService>();
builder.Services.AddScoped<ICustomTourService, CustomTourService>();
builder.Services.AddScoped<IGuideService, GuideService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IBannerService, BannerService>();
builder.Services.AddScoped<IInspirationService, InspirationService>();
builder.Services.AddHttpClient();

// Configure JWT Authentication
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseCors("AllowAll");

var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = ""
});

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
    DbInitializer.Initialize(context);
}

app.Run();
