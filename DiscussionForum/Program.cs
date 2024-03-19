using DiscussionForum.Data;
using DiscussionForum.Middleware.ExceptionLogging;
using DiscussionForum.Services;
using DiscussionForum.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;


string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Middleware", "ExceptionLogging", "ExceptionLog.txt");
Directory.CreateDirectory(Path.GetDirectoryName(_filePath));

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()
    .WriteTo.File(_filePath, rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    DotNetEnv.Env.Load();

    var connectionString = Environment.GetEnvironmentVariable("DB_STRING");
    builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngularDev",
            builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
    });

    var JwtIssuer = DotNetEnv.Env.GetString("ISSUER");
    var JwtKey = DotNetEnv.Env.GetString("KEY");
    var JwtAudience = DotNetEnv.Env.GetString("AUDIENCE");
    builder.Configuration["Jwt:JWT_Issuer"] = JwtIssuer;
    builder.Configuration["Jwt:JWT_Audience"] = JwtAudience;
    builder.Configuration["Jwt:JWT_Key"] = JwtKey;
    var jwtIssuer = builder.Configuration.GetSection("Jwt:JWT_Issuer").Value;
    var jwtAudience = builder.Configuration.GetSection("Jwt:JWT_Audience").Value;
    var jwtKey = builder.Configuration.GetSection("Jwt:JWT_Key").Value;


    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });
    /*builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Admin", policy => policy.RequireRole("SuperAdmin"));
        options.AddPolicy("Head", policy => policy.RequireRole("SuperAdmin", "CommunityHead"));
        options.AddPolicy("User", policy => policy.RequireRole("SuperAdmin", "CommunityHead", "User"));
    });*/




// Add services to the container.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDesignationService, DesignationService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ICommunityCategoryService, CommunityCategoryService>();
builder.Services.AddScoped<ICommunityStatusService, CommunityStatusService>();
builder.Services.AddScoped<IThreadStatusService, ThreadStatusService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICommunityService, CommunityService>();
builder.Services.AddScoped<ICommunityCategoryMappingService, CommunityCategoryMappingService>();
builder.Services.AddScoped<INoticeService, NoticeService>();
builder.Services.AddScoped<IThreadService, ThreadService>();
builder.Services.AddScoped<IReplyService, ReplyService>();
builder.Services.AddScoped<IPointService, PointService>();
builder.Services.AddScoped<IBadgeService, BadgeService>();
builder.Services.AddScoped<IReplyVoteService, ReplyVoteService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IMailjetService, MailjetService>();
builder.Services.AddScoped<ISavedPostService, SavedPostService>();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();


    app.UseMiddleware<RequestLoggingMiddleware>();
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("AllowAngularDev");

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    if (ex.InnerException != null)
    {
        Log.Fatal($"Error occurred. \nError: {ex.InnerException.Message}");
    }
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

