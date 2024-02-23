using DiscussionForum.Data;
using DiscussionForum.Services;
using DiscussionForum.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdmin", policy => policy.RequireRole("SuperAdmin"));
    options.AddPolicy("CommunityHead", policy => policy.RequireRole("CommunityHead"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});




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
builder.Services.AddScoped<IThreadVoteService, ThreadVoteService>();
builder.Services.AddScoped<IReplyVoteService, ReplyVoteService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IMailjetService, MailjetService>();



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



app.UseMiddleware<RequestLoggingMiddleware>();

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
